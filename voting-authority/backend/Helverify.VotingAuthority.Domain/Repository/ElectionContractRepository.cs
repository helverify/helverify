using System.Numerics;
using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Ethereum;
using Helverify.VotingAuthority.DataAccess.Ethereum.Contract;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Web3;

namespace Helverify.VotingAuthority.Domain.Repository
{
    /// <inheritdoc cref="IElectionContractRepository"/>
    public class ElectionContractRepository : IElectionContractRepository
    {
        private readonly IWeb3Loader _web3Loader;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="web3Loader">Web3 accessor</param>
        /// <param name="mapper">Automapper</param>
        public ElectionContractRepository(IWeb3Loader web3Loader, IMapper mapper)
        {
            _web3Loader = web3Loader;
            _mapper = mapper;
        }

        /// <inheritdoc cref="IElectionContractRepository.DeployContractAsync"/>
        public async Task<string> DeployContractAsync()
        {
            ElectionDeployment electionDeployment = new ElectionDeployment();

            IWeb3 web3 = _web3Loader.Web3Instance;
            
            return (await web3.Eth.GetContractDeploymentHandler<ElectionDeployment>().SendRequestAndWaitForReceiptAsync(electionDeployment)).ContractAddress;
        }

        /// <inheritdoc cref="IElectionContractRepository.SetUpAsync"/>
        public async Task SetUpAsync(Election election)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

            SetUpFunction setUpFunction = new SetUpFunction
            {
                Id = election.Id!,
                Candidates = election.Options.Select(o => o.Name).ToList(),
                Parameters = new ElGamalParameters
                {
                    P = election.P.ConvertToHexString(),
                    G = election.G.ConvertToHexString()
                },
                ElectionPublicKey = election.PublicKey.ConvertToHexString(),
                ConsensusPublicKeys = election.Blockchain.Registrations
                    .Select(r => r.PublicKeys[election.Id!].ConvertToHexString()).ToList()
            };

            await contract.SendRequestAndWaitForReceiptAsync(setUpFunction);
        }

        /// <inheritdoc cref="IElectionContractRepository.StoreBallotsAsync"/>
        public async Task StoreBallotsAsync(Election election, IList<Model.Paper.PaperBallot> paperBallots)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

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

            await WithRetry(async() => await contract.SendRequestAsync(storeBallotFunction));
        }

        /// <summary>
        /// Retry pattern according to: https://docs.microsoft.com/en-us/azure/architecture/patterns/retry
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private async Task WithRetry(Func<Task> action)
        {
            int tries = 0;

            while (true)
            {
                try
                {
                    await action();

                    break;
                }
                catch (Exception)
                {
                    if (tries > 10)
                    {
                        throw;
                    }

                    await Task.Delay(2000);
                    tries++;
                }
            }
        }

        /// <inheritdoc cref="IElectionContractRepository.GetBallotIdsAsync"/>
        public async Task<Tuple<IList<string>, int>> GetBallotIdsAsync(Election election, int startIndex, int partitionSize)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

            GetAllBallotIdsFunction getAllBallotIdsFunction = new GetAllBallotIdsFunction
            {
                StartIndex = startIndex,
                PartitionSize = partitionSize

            };

            GetAllBallotIdsOutputDTO result = await contract.QueryAsync<GetAllBallotIdsFunction, GetAllBallotIdsOutputDTO>(getAllBallotIdsFunction);

            IList<string> ballotIds = result.ReturnValue1;
            int currentIndex = (int)result.ReturnValue2;

            return new Tuple<IList<string>, int>(ballotIds, currentIndex);
        }

        /// <inheritdoc cref="IElectionContractRepository.GetBallotAsync"/>
        public async Task<IList<PublishedBallot>> GetBallotAsync(Election election, string id)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

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

        /// <inheritdoc cref="IElectionContractRepository.PublishBallotSelectionAsync"/>
        public async Task PublishBallotSelectionAsync(Election election, string id, string ballotCode, IList<string> shortCodes)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

            PublishBallotSelectionFunction publishShortCodesFunction = new PublishBallotSelectionFunction
            {
                BallotId = id,
                BallotCode = ballotCode,
                ShortCodes = shortCodes.ToList()
            };

            await contract.SendRequestAsync(publishShortCodesFunction);
        }

        /// <inheritdoc cref="IElectionContractRepository.SpoilBallotAsync"/>
        public async Task SpoilBallotAsync(string ballotId, string virtualBallotId, Election election, string ipfsCid)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

            SpoilBallotFunction spoilBallotFunction = new SpoilBallotFunction
            {
                BallotId = ballotId,
                VirtualBallotId = virtualBallotId,
                SpoiltBallotIpfs = ipfsCid
            };

            await contract.SendRequestAsync(spoilBallotFunction);
        }

        /// <inheritdoc cref="IElectionContractRepository.GetNumberOfBallotsAsync"/>
        public async Task<int> GetNumberOfBallotsAsync(Election election)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

            GetNumberOfBallotsFunction getNumberOfBallotsFunction = new GetNumberOfBallotsFunction();

            int numberOfBallots = await contract.QueryAsync<GetNumberOfBallotsFunction, int>(getNumberOfBallotsFunction);

            return numberOfBallots;
        }

        /// <inheritdoc cref="IElectionContractRepository.GetNumberOfCastBallotsAsync"/>
        public async Task<int> GetNumberOfCastBallotsAsync(Election election)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

            GetNumberOfCastBallotsFunction getNumberOfCastBallotsFunction = new GetNumberOfCastBallotsFunction();

            int numberOfCastBallots = await contract.QueryAsync<GetNumberOfCastBallotsFunction, int>(getNumberOfCastBallotsFunction);

            return numberOfCastBallots;
        }

        /// <inheritdoc cref="IElectionContractRepository.GetCastBallotAsync"/>
        public async Task<Tuple<PublishedBallot, IList<string>>> GetCastBallotAsync(Election election, string ballotId)
        {
            ContractHandler contract = GetContractHandlerAsync(election);
            
            RetrieveCastBallotFunction castBallotsFunction = new RetrieveCastBallotFunction
            {
                BallotId = ballotId
            };

            RetrieveCastBallotOutputDTO result = await contract.QueryAsync<RetrieveCastBallotFunction, RetrieveCastBallotOutputDTO>(castBallotsFunction);
            
            CastBallot castBallot = result.ReturnValue1;

            PublishedBallot ballot = new()
            {
                BallotId = castBallot.BallotId,
                BallotCode = castBallot.BallotCode,
                IpfsCid = castBallot.BallotIpfs,
            };

            IList<string> selection = castBallot.Selection;

            return new Tuple<PublishedBallot, IList<string>>(ballot, selection);
        }

        /// <inheritdoc cref="IElectionContractRepository.PublishResults"/>
        public async Task PublishResults(Election election, IList<DecryptedValue> results, string evidenceCid)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

            List<Result> contractResults = new List<Result>();

            for (int i = 0; i < results.Count; i++)
            {
                contractResults.Add(new Result
                {
                    Option = election.Options[i].Name,
                    Tally = new BigInteger(results[i].PlainText)
                });
            }

            PublishResultFunction publishResultFunction = new PublishResultFunction
            {
                TallyResults = contractResults,
                TallyProofsIpfs = evidenceCid
            };

            await contract.SendRequestAsync(publishResultFunction);
        }

        /// <inheritdoc cref="IElectionContractRepository.GetResultsAsync"/>
        public async Task<ElectionResults> GetResultsAsync(Election election)
        {
            ContractHandler contract = GetContractHandlerAsync(election);

            GetResultsFunction getResultsFunction = new GetResultsFunction();

            try
            {
                GetResultsOutputDTO getResultsOutputDto =
                    await contract.QueryAsync<GetResultsFunction, GetResultsOutputDTO>(getResultsFunction);

                List<Result> resultsDto = getResultsOutputDto.ReturnValue1;

                IList<ElectionResult> electionResults = _mapper.Map<IList<ElectionResult>>(resultsDto);

                return new ElectionResults(electionResults);
            }
            catch (Exception)
            {
                return new ElectionResults(new List<ElectionResult>());
            }
        }

        private ContractHandler GetContractHandlerAsync(Election election)
        {
            IWeb3 web3 = _web3Loader.Web3Instance;

            return web3.Eth.GetContractHandler(election.ContractAddress);
        }
    }
}
