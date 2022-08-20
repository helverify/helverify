using System.Collections.Concurrent;
using Helverify.Cryptography.Encryption;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.DataAccess.Ethereum;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Model.Consensus;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Application.Services
{
    /// <inheritdoc cref="IElectionService"/>
    internal class ElectionService : IElectionService
    {
        private readonly IRepository<Election> _electionRepository;
        private readonly IRepository<Blockchain> _bcRepository;
        private readonly IConsensusNodeService _consensusNodeService;
        private readonly IElectionContractRepository _contractRepository;
        private readonly IPublishedBallotRepository _publishedBallotRepository;
        private readonly IWeb3Loader _web3Loader;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="electionRepository">Data access to elections</param>
        /// <param name="bcRepository">Data access to blockchain configuration</param>
        /// <param name="consensusNodeService">Interaction with consensus nodes</param>
        /// <param name="contractRepository">Interaction with the smart contract</param>
        /// <param name="publishedBallotRepository">Interaction with IPFS</param>
        /// <param name="web3Loader">Web3 blockchain connector</param>
        public ElectionService(IRepository<Election> electionRepository, IRepository<Blockchain> bcRepository, IConsensusNodeService consensusNodeService,
            IElectionContractRepository contractRepository, IPublishedBallotRepository publishedBallotRepository, IWeb3Loader web3Loader)
        {
            _electionRepository = electionRepository;
            _bcRepository = bcRepository;
            _consensusNodeService = consensusNodeService;
            _contractRepository = contractRepository;
            _publishedBallotRepository = publishedBallotRepository;
            _web3Loader = web3Loader;
        }

        /// <inheritdoc cref="IElectionService.CreateAsync"/>
        public async Task<Election> CreateAsync(Election election)
        {
            election = await _electionRepository.CreateAsync(election);

            return election;
        }

        /// <inheritdoc cref="IElectionService.GetAsync(string)"/>
        public async Task<Election> GetAsync(string electionId)
        {
            Election election = await _electionRepository.GetAsync(electionId);

            election.Blockchain = await _bcRepository.GetAsync(election.Blockchain.Id);

            return election;
        }

        /// <inheritdoc cref="IElectionService.GetAsync()"/>
        public async Task<IList<Election>> GetAsync() => await _electionRepository.GetAsync();

        /// <inheritdoc cref="IElectionService.UpdateAsync"/>
        public async Task<Election> UpdateAsync(string electionId, Election election) => await _electionRepository.UpdateAsync(electionId, election);

        /// <inheritdoc cref="IElectionService.DeleteAsync"/>
        public async Task DeleteAsync(string electionId) => await _electionRepository.DeleteAsync(electionId);

        /// <inheritdoc cref="IElectionService.GeneratePublicKeyAsync"/>
        public async Task<Election> GeneratePublicKeyAsync(string electionId)
        {
            Election election = await GetAsync(electionId);

            if (election.Id == null)
            {
                throw new NullReferenceException("Election id is null");
            }

            IList<Registration> registrations = election.Blockchain.Registrations;

            foreach (Registration registration in registrations)
            {
                PublicKeyDto publicKey = await _consensusNodeService.GenerateKeyPairAsync(registration.Endpoint, election) ?? throw new NullReferenceException("Public key is null");

                registration.SetPublicKey(publicKey, election);
            }

            await _bcRepository.UpdateAsync(election.Blockchain.Id, election.Blockchain);

            election.CombinePublicKeys(registrations.Select(r => r.PublicKeys[election.Id]).ToList());

            election = await UpdateAsync(election.Id, election);

            return election;
        }

        /// <inheritdoc cref="IElectionService.DeployElectionContractAsync"/>
        public async Task<Election> DeployElectionContractAsync(string electionId)
        {
            _web3Loader.LoadInstance();

            Election election = await GetAsync(electionId);

            election.ContractAddress = await _contractRepository.DeployContractAsync();

            await _electionRepository.UpdateAsync(electionId, election);

            await _contractRepository.SetUpAsync(election);

            return election;
        }

        /// <inheritdoc cref="IElectionService.CalculateTallyAsync"/>
        public async Task<IList<DecryptedValue>> CalculateTallyAsync(string electionId)
        {
            
            Election election = await GetAsync(electionId);

            int numberOfBallots = await _contractRepository.GetNumberOfBallotsAsync(election);
            
            int index = 0;
            int partitionSize = 100;

            IList<EncryptedOption> selectedEncryptedOptions = await GetEncryptedOptionsAsync(index, numberOfBallots, election, partitionSize);
            
            Tally tally = new Tally(selectedEncryptedOptions);

            IList<ElGamalCipher> encryptedResults = tally.CalculateCipherResult(election);
            
            IList<DecryptedValue> results = new List<DecryptedValue>();

            foreach (ElGamalCipher cipher in encryptedResults)
            {
                DecryptedValue decryptedValue = await DecryptAsync(election, cipher);

                results.Add(decryptedValue);
            }
            
            string evidenceCid = _publishedBallotRepository.StoreDecryptedResults(results);
            
            await _contractRepository.PublishResults(election, results, evidenceCid);
            
            return results;
        }

        /// <inheritdoc cref="IElectionService.GetResultsAsync"/>
        public async Task<ElectionResults> GetResultsAsync(string electionId)
        {
            Election election = await GetAsync(electionId);

            ElectionResults electionResults = await _contractRepository.GetResultsAsync(election);

            return electionResults;
        }

        /// <inheritdoc cref="IElectionService.GetElectionNumbersAsync"/>
        public async Task<ElectionNumbers> GetElectionNumbersAsync(string electionId)
        {
            Election election = await GetAsync(electionId);

            int numberOfBallots = await _contractRepository.GetNumberOfBallotsAsync(election);
            int numberOfCastBallots = await _contractRepository.GetNumberOfCastBallotsAsync(election);

            return new ElectionNumbers(numberOfBallots, numberOfCastBallots);
        }

        /// <summary>
        /// Decrypts a single ciphertext cooperatively
        /// </summary>
        /// <param name="election">Current Election</param>
        /// <param name="cipher">ElGamal ciphertext</param>
        /// <returns></returns>
        private async Task<DecryptedValue> DecryptAsync(Election election, ElGamalCipher cipher)
        {
            IList<Registration> consensusNodes = election.Blockchain.Registrations;

            IList<DecryptedShare> shares = new List<DecryptedShare>();

            foreach (Registration node in consensusNodes)
            {
                DecryptedShare share = await _consensusNodeService.DecryptShareAsync(node.Endpoint, election, cipher, node.PublicKeys[election.Id!]);

                bool isValid = share.ProofOfDecryption.Verify(cipher.C, cipher.D, new DHPublicKeyParameters(share.PublicKeyShare, election.DhParameters));

                if (!isValid)
                {
                    throw new Exception("Decryption proof is invalid");
                }

                shares.Add(share);
            }

            int plainText = election.CombineShares(shares, cipher.D);

            return new DecryptedValue
            {
                PlainText = plainText,
                CipherText = cipher,
                Shares = shares
            };
        }

        private async Task<IList<EncryptedOption>> GetEncryptedOptionsAsync(int index, int numberOfBallots,
            Election election, int partitionSize)
        {
            ConcurrentQueue<EncryptedOption> selectedEncryptedOptions = new ConcurrentQueue<EncryptedOption>();

            while (index < numberOfBallots)
            {
                Tuple<IList<string>, int> result =
                    await _contractRepository.GetBallotIdsAsync(election, index, partitionSize);

                await Parallel.ForEachAsync(result.Item1, async (ballotId, _) =>
                {
                    if (string.IsNullOrEmpty(ballotId))
                    {
                        return;
                    }

                    Tuple<PublishedBallot, IList<string>> ballotResult =
                        await _contractRepository.GetCastBallotAsync(election, ballotId);
                
                    if (!string.IsNullOrEmpty(ballotResult.Item1.IpfsCid))
                    {
                        VirtualBallot virtualBallot =
                            _publishedBallotRepository.RetrieveVirtualBallot(ballotResult.Item1.IpfsCid);
                        
                        Parallel.ForEach(virtualBallot.GetSelectedEncryptions(ballotResult.Item2),
                            (selectedEncryption, _) => { selectedEncryptedOptions.Enqueue(selectedEncryption); });
                    }
                });

                index += partitionSize;
            }

            return selectedEncryptedOptions.ToList();
        }
    }
}
