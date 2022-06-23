using Helverify.VotingAuthority.DataAccess.Ethereum.Contract;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository;

public interface IElectionContractRepository
{
    Task<string> DeployContract();
    Task SetUp(Election election);
    Task StoreBallots(Election election, IList<PaperBallot> paperBallots);
    Task<IList<string>> GetBallotIds(Election election);
    Task<PaperBallot> GetBallot(Election election, string id);
}