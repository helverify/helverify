using System.Collections.Concurrent;
using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;

namespace Helverify.VotingAuthority.Application.Services
{
    /// <inheritdoc cref="IBallotService"/>
    internal class BallotService : IBallotService
    {
        private readonly IConsensusNodeService _consensusNodeService;
        private readonly IRepository<PaperBallot> _ballotRepository;
        private readonly IRepository<Blockchain> _bcRepository;
        private readonly IPublishedBallotRepository _publishedBallotRepository;
        private readonly IElectionContractRepository _contractRepository;
        private readonly IMapper _mapper;
        private readonly Random _random;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consensusNodeService">Provides access to consensus nodes</param>
        /// <param name="ballotRepository">Data access to ballots</param>
        /// <param name="bcRepository">Data access to blockchain configuration</param>
        /// <param name="publishedBallotRepository">Interaction with IPFS</param>
        /// <param name="contractRepository">Interaction with smart contract</param>
        /// <param name="mapper">Automapper</param>
        public BallotService(IConsensusNodeService consensusNodeService, IRepository<PaperBallot> ballotRepository, IRepository<Blockchain> bcRepository, IPublishedBallotRepository publishedBallotRepository, IElectionContractRepository contractRepository, IMapper mapper)
        {
            _consensusNodeService = consensusNodeService;
            _ballotRepository = ballotRepository;
            _bcRepository = bcRepository;
            _publishedBallotRepository = publishedBallotRepository;
            _contractRepository = contractRepository;
            _mapper = mapper;
            _random = new Random();
        }

        /// <inheritdoc cref="IBallotService.GetAsync(string {id})"/>
        public async Task<PaperBallot> GetAsync(string ballotId) => await _ballotRepository.GetAsync(ballotId);

        /// <inheritdoc cref="IBallotService.GetAsync(int {numberOfBallots})"/>
        public async Task<IList<PaperBallot>> GetAsync(Election election, int numberOfBallots)
        {
            return (await (_ballotRepository as PaperBallotRepository).GetByElectionAsync(election))
                .Where(b => !b.Printed).Take(numberOfBallots).ToList();
        }

        /// <inheritdoc cref="IBallotService.CreateBallots"/>
        public async Task CreateBallots(Election election, int numberOfBallots)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-mm-dd HH:mm:ss.fff} BEGIN BALLOT CREATE {numberOfBallots}");
            
            BallotTemplate ballotTemplate = election.GenerateBallotTemplate();

            int partitionSize = 100;

            for (int i = 0; i < numberOfBallots; i += partitionSize)
            {
                ConcurrentQueue<PaperBallot> paperBallots = new ConcurrentQueue<PaperBallot>();

                int endIndex = (i + partitionSize) <= numberOfBallots ? partitionSize : (numberOfBallots % partitionSize);

                Parallel.For(0, endIndex, (_) =>
                {
                    VirtualBallot ballot1 = ballotTemplate.Encrypt();
                    VirtualBallot ballot2 = ballotTemplate.Encrypt();

                    PaperBallot paperBallot = new PaperBallot(election, ballot1, ballot2);

                    _publishedBallotRepository.StoreVirtualBallot(ballot1);
                    _publishedBallotRepository.StoreVirtualBallot(ballot2);

                    paperBallots.Enqueue(paperBallot);
                });

                await (_ballotRepository as PaperBallotRepository)!.InsertMany(paperBallots.ToArray());

                await _contractRepository.StoreBallotsAsync(election, paperBallots.ToList());
            }

            Console.WriteLine($"{DateTime.Now:yyyy-mm-dd HH:mm:ss.fff} END BALLOT CREATE {numberOfBallots}");
        }

        /// <inheritdoc cref="IBallotService.PublishBallotEvidence"/>
        public async Task PublishBallotEvidence(Election election, string ballotId, int spoiltBallotIndex, IList<string> selectedOptions)
        {
            PaperBallot paperBallot = await _ballotRepository.GetAsync(ballotId);

            paperBallot.Election = election;

            PublishedBallot ballot = (await _contractRepository.GetBallotAsync(election, ballotId))[1 - spoiltBallotIndex];

            try
            {
                await PublishSelections(paperBallot, selectedOptions, ballot.BallotCode);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }

            VirtualBallot virtualBallot = await DecryptBallot(election, ballotId, spoiltBallotIndex);

            string cid = _publishedBallotRepository.StoreSpoiltBallot(virtualBallot, paperBallot.GetRandomness(spoiltBallotIndex));

            await _contractRepository.SpoilBallotAsync(paperBallot.BallotId, virtualBallot.Code, election, cid);
        }

        /// <inheritdoc cref="IBallotService.PublishRandomEvidence"/>
        public async Task PublishRandomEvidence(Election election)
        {
            int numberOfBallots = await _contractRepository.GetNumberOfBallotsAsync(election);

            int index = 0;
            int partitionSize = 100;

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

                    PaperBallot paperBallot = await _ballotRepository.GetAsync(ballotId);

                    int spoiltBallotIndex = _random.Next(0, 2);

                    int selectedOption = _random.Next(0, paperBallot.Options.Count);

                    PaperBallotOption option = paperBallot.Options[selectedOption];

                    string shortCode = spoiltBallotIndex == 1 ? option.ShortCode1 : option.ShortCode2;

                    paperBallot.Election = election;

                    PublishedBallot ballot = (await _contractRepository.GetBallotAsync(election, ballotId))[1 - spoiltBallotIndex];

                    try
                    {
                        await PublishSelections(paperBallot, new List<string> { shortCode }, ballot.BallotCode);
                    }
                    catch (ArgumentException ex)
                    {
                        throw ex;
                    }
                });

                index += partitionSize;
            }
        }

        private async Task<VirtualBallot> DecryptBallot(Election election, string ballotId, int spoiltBallotIndex)
        {
            PublishedBallot publishedBallot = (await _contractRepository.GetBallotAsync(election, ballotId))[spoiltBallotIndex];

            VirtualBallot virtualBallot = _publishedBallotRepository.RetrieveVirtualBallot(publishedBallot.IpfsCid);

            virtualBallot = await CoopDecryptBallot(virtualBallot, election, publishedBallot.IpfsCid);

            return virtualBallot;
        }

        private async Task PublishSelections(PaperBallot paperBallot, IList<string> selectedOptions, string ballotCode)
        {
            IList<string> selection = selectedOptions.OrderBy(s => s).ToList();

            if (!paperBallot.HasShortCodes(selection))
            {
                throw new ArgumentNullException(nameof(selectedOptions), "Selection is not valid");
            }

            await _contractRepository.PublishBallotSelectionAsync(paperBallot.Election, paperBallot.BallotId, ballotCode, selection);
        }

        private async Task<VirtualBallot> CoopDecryptBallot(VirtualBallot ballot, Election election, string ipfsCid)
        {
            List<OptionShare> optionShares = await GetOptionShares(election, ballot, ipfsCid);

            BallotShares ballotShares = new BallotShares(optionShares);

            ballot = ballotShares.CombineShares(election, ballot);

            return ballot;
        }

        private async Task<List<OptionShare>> GetOptionShares(Election election, VirtualBallot ballot, string ipfsCid)
        {
            ConcurrentQueue<OptionShare> optionShares = new ConcurrentQueue<OptionShare>();

            Parallel.ForEach(election.Blockchain.Registrations, (consensusNode) =>
            {
                DecryptedBallotShareDto? decryptedBallot = _consensusNodeService.DecryptBallotAsync(consensusNode.Endpoint, ballot, election.Id!, ipfsCid).Result;

                if (decryptedBallot == null)
                {
                    throw new NullReferenceException("Decrypted ballot share must not be null.");
                }

                decryptedBallot.PublicKey = consensusNode.PublicKeys[election.Id!];

                IList<OptionShare> optionSharesPart = _mapper.Map<IList<OptionShare>>(decryptedBallot);

                foreach (OptionShare optionShare in optionSharesPart)
                {
                    optionShares.Enqueue(optionShare);
                }
            });

            return optionShares.ToList();
        }
    }
}
