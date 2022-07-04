using Helverify.VotingAuthority.DataAccess.Ethereum;
using Helverify.VotingAuthority.DataAccess.Ethereum.Contract;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Web3;

namespace Helverify.VotingAuthority.Domain.Repository
{
    /// <inheritdoc cref="IElectionContractRepository"/>
    public class ElectionContractRepository : IElectionContractRepository
    {
        private readonly IWeb3Loader _web3Loader;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="web3Loader">Web3 accessor</param>
        /// <param name="mapper">Automapper</param>
        public ElectionContractRepository(IWeb3Loader web3Loader)
        {
            _web3Loader = web3Loader;
        }

        /// <inheritdoc cref="IElectionContractRepository.DeployContractAsync"/>
        public async Task<string> DeployContractAsync()
        {
            ElectionDeployment electionDeployment = new ElectionDeployment();

            IWeb3 web3 = _web3Loader.Web3Instance;

            await UnlockAccountAsync();

            return (await web3.Eth.GetContractDeploymentHandler<ElectionDeployment>().SendRequestAndWaitForReceiptAsync(electionDeployment)).ContractAddress;
        }

        /// <inheritdoc cref="IElectionContractRepository.SetUpAsync"/>
        public async Task SetUpAsync(Election election)
        {
            IWeb3 web3 = _web3Loader.Web3Instance;

            await UnlockAccountAsync();

            ContractHandler contract = web3.Eth.GetContractHandler(election.ContractAddress);
            
            SetUpFunction setUpFunction = new SetUpFunction
            {
                Id = election.Id,
                Candidates = election.Options.Select(o => o.Name).ToList()
            };

            await contract.SendRequestAndWaitForReceiptAsync(setUpFunction);
        }

        /// <inheritdoc cref="IElectionContractRepository.StoreBallotsAsync"/>
        public async Task StoreBallotsAsync(Election election, IList<Model.Paper.PaperBallot> paperBallots)
        {
            IWeb3 web3 = _web3Loader.Web3Instance;

            await UnlockAccountAsync();

            ContractHandler contract = web3.Eth.GetContractHandler(election.ContractAddress);

            List<PaperBallot> ballots = paperBallots.Select(pb =>
            {
                VirtualBallot ballot1 = pb.Ballots[0];
                VirtualBallot ballot2 = pb.Ballots[1];

                return new PaperBallot
                {
                    Ballot1Code = ballot1.Code,
                    Ballot1Ipfs = ballot1.IpfsCid,
                    Ballot2Code = ballot2.Code,
                    Ballot2Ipfs = ballot2.IpfsCid,
                    BallotId = pb.BallotId
                };
            }).ToList();

            StoreBallotFunction storeBallotFunction = new StoreBallotFunction
            {
                Ballots = ballots
            };

            await contract.SendRequestAsync(storeBallotFunction);
        }

        /// <inheritdoc cref="IElectionContractRepository.GetBallotIdsAsync"/>
        public async Task<IList<string>> GetBallotIdsAsync(Election election)
        {
            IWeb3 web3 = _web3Loader.Web3Instance;

            await UnlockAccountAsync();

            ContractHandler contract = web3.Eth.GetContractHandler(election.ContractAddress);

            GetNumberOfBallotsFunction getNumberOfBallotsFunction = new GetNumberOfBallotsFunction();

            int numberOfBallots = await contract.QueryAsync<GetNumberOfBallotsFunction, int>(getNumberOfBallotsFunction);

            IList<string> ballotIds = new List<string>();

            for (int i = 0; i < numberOfBallots; i++)
            {
                BallotIdsFunction ballotIdsFunction = new BallotIdsFunction
                {
                    ReturnValue1 = i
                };

                string ballotId = await contract.QueryAsync<BallotIdsFunction, string>(ballotIdsFunction);

                ballotIds.Add(ballotId);
            }

            return ballotIds;
        }

        /// <inheritdoc cref="IElectionContractRepository.GetBallotAsync"/>
        public async Task<IList<PublishedBallot>> GetBallotAsync(Election election, string id)
        {
            IWeb3 web3 = _web3Loader.Web3Instance;

            await UnlockAccountAsync();

            ContractHandler contract = web3.Eth.GetContractHandler(election.ContractAddress);

            RetrieveBallotFunction retrieveBallotFunction = new RetrieveBallotFunction
            {
                BallotId = id
            };

            PaperBallot paperBallot = (await contract.QueryDeserializingToObjectAsync<RetrieveBallotFunction, RetrieveBallotOutputDTO>(retrieveBallotFunction)).ReturnValue1;
            
            return new List<PublishedBallot>
            {
                new ()
                {
                    BallotId = paperBallot.BallotId,
                    BallotCode = paperBallot.Ballot1Code,
                    IpfsCid = paperBallot.Ballot1Ipfs
                },
                new ()
                {
                    BallotId = paperBallot.BallotId,
                    BallotCode = paperBallot.Ballot2Code,
                    IpfsCid = paperBallot.Ballot2Ipfs
                }
            };
        }

        /// <inheritdoc cref="IElectionContractRepository.PublishShortCodesAsync"/>
        public async Task PublishShortCodesAsync(Election election, string id, IList<string> shortCodes)
        {
            IWeb3 web3 = _web3Loader.Web3Instance;

            await UnlockAccountAsync();

            ContractHandler contract = web3.Eth.GetContractHandler(election.ContractAddress);

            PublishShortCodesFunction publishShortCodesFunction = new PublishShortCodesFunction
            {
                BallotId = id,
                ShortCodes = shortCodes.ToList()
            };

            await contract.SendRequestAsync(publishShortCodesFunction);
        }

        /// <inheritdoc cref="IElectionContractRepository.SpoilBallotAsync"/>
        public async Task SpoilBallotAsync(string ballotId, string virtualBallotId, Election election, string ipfsCid)
        {
            IWeb3 web3 = _web3Loader.Web3Instance;

            await UnlockAccountAsync();

            ContractHandler contract = web3.Eth.GetContractHandler(election.ContractAddress);

            SpoilBallotFunction spoilBallotFunction = new SpoilBallotFunction
            {
                BallotId = ballotId,
                VirtualBallotId = virtualBallotId,
                SpoiltBallotIpfs = ipfsCid
            };

            await contract.SendRequestAsync(spoilBallotFunction);
        }

        private async Task UnlockAccountAsync(int seconds = 120)
        {
            IWeb3 web3 = _web3Loader.Web3Instance;

            await web3.Personal.UnlockAccount.SendRequestAsync(_web3Loader.Account.Address, _web3Loader.Password, 120);
        }
    }
}
