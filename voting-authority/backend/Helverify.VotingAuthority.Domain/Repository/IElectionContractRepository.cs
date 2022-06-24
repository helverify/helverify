using Helverify.VotingAuthority.DataAccess.Ethereum.Contract;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository;

/// <summary>
/// Manages access to the Election smart contract.
/// </summary>
public interface IElectionContractRepository
{
    /// <summary>
    /// Deploys the election contract to the Blockchain
    /// </summary>
    /// <returns></returns>
    Task<string> DeployContract();

    /// <summary>
    /// Initializes the options / candidates on the smart contract.
    /// </summary>
    /// <param name="election">Election</param>
    /// <returns></returns>
    Task SetUp(Election election);

    /// <summary>
    /// Stores a list of PaperBallot (list of tuple(ballotId, ballot1Code, ballot1Ipfs, ballot2Code, ballot2Ipfs)) parameters on the smart contract.
    /// </summary>
    /// <param name="election">Election</param>
    /// <param name="paperBallots">PaperBallot data</param>
    /// <returns></returns>
    Task StoreBallots(Election election, IList<PaperBallot> paperBallots);

    /// <summary>
    /// Retrieves all ballotIds for the specified election.
    /// </summary>
    /// <param name="election">Election</param>
    /// <returns></returns>
    Task<IList<string>> GetBallotIds(Election election);

    /// <summary>
    /// Retrieves a single ballot from the smart contract, containing a tuple (ballotId, ballot1Code, ballot1Ipfs, ballot2Code, ballot2Ipfs)
    /// </summary>
    /// <param name="election">Election</param>
    /// <param name="id">Ballot ID</param>
    /// <returns></returns>
    Task<PaperBallot> GetBallot(Election election, string id);
}