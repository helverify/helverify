using Helverify.VotingAuthority.DataAccess.Ethereum.Contract;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository;

public interface IElectionContractRepository
{
    Task<string> DeployContract();
    Task SetUp(Election election);
    Task StoreBallot(Election election, string ballotId, string ballot1Id, string ballot1Cid, string ballot2Id, string ballot2Cid);
    Task<IList<string>> GetBallotIds(Election election);
    Task<PaperBallot> GetBallot(Election election, string id);
}