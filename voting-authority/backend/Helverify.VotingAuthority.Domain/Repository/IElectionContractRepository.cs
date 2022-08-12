using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Model.Virtual;

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
    Task<string> DeployContractAsync();

    /// <summary>
    /// Initializes the options / candidates on the smart contract.
    /// </summary>
    /// <param name="election">Election</param>
    /// <returns></returns>
    Task SetUpAsync(Election election);

    /// <summary>
    /// Stores a list of PaperBallot (list of tuple(ballotId, ballot1Code, ballot1Ipfs, ballot2Code, ballot2Ipfs)) parameters on the smart contract.
    /// </summary>
    /// <param name="election">Election</param>
    /// <param name="paperBallots">PaperBallot data</param>
    /// <returns></returns>
    Task StoreBallotsAsync(Election election, IList<PaperBallot> paperBallots);

    /// <summary>
    /// Retrieves all ballotIds for the specified election.
    /// </summary>
    /// <param name="election">Election</param>
    /// <param name="startIndex">Ballot ID index to start reading from</param>
    /// <param name="partitionSize">Number of elements to read</param>
    /// <returns></returns>
    Task<Tuple<IList<string>, int>> GetBallotIdsAsync(Election election, int startIndex, int partitionSize);

    /// <summary>
    /// Retrieves a single ballot from the smart contract, containing a tuple (ballotId, ballot1Code, ballot1Ipfs, ballot2Code, ballot2Ipfs)
    /// </summary>
    /// <param name="election">Election</param>
    /// <param name="id">Ballot ID</param>
    /// <returns></returns>
    Task<IList<PublishedBallot>> GetBallotAsync(Election election, string id);

    /// <summary>
    /// Publishes the short codes of the selected options of one ballot.
    /// </summary>
    /// <param name="election">Election</param>
    /// <param name="id">Ballot ID</param>
    /// <param name="ballotCode">Code of the virtual ballot</param>
    /// <param name="shortCodes">Selected short codes of the non-spoilt ballot.</param>
    /// <returns></returns>
    Task PublishBallotSelectionAsync(Election election, string id, string ballotCode, IList<string> shortCodes);


    /// <summary>
    /// Marks a ballot as spoilt on the SC and publishes the IPFS cid to the spoilt ballot.
    /// </summary>
    /// <param name="ballotId">ID of the corresponding Paper Ballot</param>
    /// <param name="virtualBallotId">Code of the ballot to be spoilt</param>
    /// <param name="election">Election</param>
    /// <param name="ipfsCid">IPFS Cid</param>
    /// <returns></returns>
    Task SpoilBallotAsync(string ballotId, string virtualBallotId, Election election, string ipfsCid);

    /// <summary>
    /// Returns the number of ballots registered on the smart contract.
    /// </summary>
    /// <param name="election">Election</param>
    /// <returns></returns>
    Task<int> GetNumberOfBallotsAsync(Election election);

    /// <summary>
    /// Returns the number of cast ballots registered on the smart contract.
    /// </summary>
    /// <param name="election">Election</param>
    /// <returns></returns>
    Task<int> GetNumberOfCastBallotsAsync(Election election);

    /// <summary>
    /// Returns the selection and storage location of a cast ballot.
    /// </summary>
    /// <param name="ballotId">Ballot identifier</param>
    /// <returns></returns>
    Task<Tuple<PublishedBallot, IList<string>>> GetCastBallotAsync(Election election, string ballotId);

    /// <summary>
    /// Publishes the decrypted results on the smart contract.
    /// </summary>
    /// <param name="results">Result values</param>
    /// <param name="evidenceCid">Evidence location on IFPS</param>
    /// <returns></returns>
    Task PublishResults(Election election, IList<DecryptedValue> results, string evidenceCid);

    /// <summary>
    /// Retrieves the final tally (results) from the smart contract.
    /// </summary>
    /// <param name="election">Current election</param>
    /// <returns></returns>
    Task<ElectionResults> GetResultsAsync(Election election);
}