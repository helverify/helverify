using Helverify.VotingAuthority.DataAccess.Ethereum;
using Helverify.VotingAuthority.DataAccess.Ethereum.Contract;
using Helverify.VotingAuthority.Domain.Model;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Web3;

namespace Helverify.VotingAuthority.Domain.Repository
{
    public class ElectionContractRepository : IElectionContractRepository
    {
        private readonly IWeb3Loader _web3Loader;

        public ElectionContractRepository(IWeb3Loader web3Loader)
        {
            _web3Loader = web3Loader;
        }

        public async Task<string> DeployContract()
        {
            ElectionDeployment electionDeployment = new ElectionDeployment();

            Web3 web3 = _web3Loader.Web3Instance;

            await UnlockAccount();

            return (await web3.Eth.GetContractDeploymentHandler<ElectionDeployment>().SendRequestAndWaitForReceiptAsync(electionDeployment)).ContractAddress;
        }

        public async Task SetUp(Election election)
        {
            Web3 web3 = _web3Loader.Web3Instance;

            await UnlockAccount();

            ContractHandler contract = web3.Eth.GetContractHandler(election.ContractAddress);
            
            SetUpFunction setUpFunction = new SetUpFunction
            {
                Id = election.Id,
                Candidates = election.Options.Select(o => o.Name).ToList()
            };

            await contract.SendRequestAndWaitForReceiptAsync(setUpFunction);
        }

        public async Task StoreBallot(Election election, string ballotId, string ballot1Id, string ballot1Cid, string ballot2Id, string ballot2Cid)
        {
            Web3 web3 = _web3Loader.Web3Instance;

            await UnlockAccount();

            ContractHandler contract = web3.Eth.GetContractHandler(election.ContractAddress);

            StoreBallotFunction storeBallotFunction = new StoreBallotFunction
            {
                BallotId = ballotId,
                BallotCode1 = ballot1Id,
                Ballot1Ipfs = ballot1Cid,
                BallotCode2 = ballot2Id,
                Ballot2Ipfs = ballot2Cid
            };

            await contract.SendRequestAndWaitForReceiptAsync(storeBallotFunction);
        }

        public async Task<IList<string>> GetBallotIds(Election election)
        {
            Web3 web3 = _web3Loader.Web3Instance;

            await UnlockAccount();

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

        private async Task UnlockAccount(int seconds = 120)
        {
            Web3 web3 = _web3Loader.Web3Instance;

            await web3.Personal.UnlockAccount.SendRequestAsync(_web3Loader.Account.Address, _web3Loader.Password, 120);
        }
    }
}
