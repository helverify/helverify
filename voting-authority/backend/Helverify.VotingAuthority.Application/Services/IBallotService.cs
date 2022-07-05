using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;

namespace Helverify.VotingAuthority.Application.Services;

/// <summary>
/// Facade for ballot domain logic.
/// </summary>
public interface IBallotService
{
    /// <summary>
    /// Retrieves a paper ballot by its id.
    /// </summary>
    /// <param name="ballotId">Ballot identifier</param>
    /// <returns></returns>
    Task<PaperBallot> GetAsync(string ballotId);

    /// <summary>
    /// Returns the specified number of unprinted ballots
    /// </summary>
    /// <param name="numberOfBallots">Number of ballots to print</param>
    /// <returns></returns>
    Task<IList<PaperBallot>> GetAsync(int numberOfBallots);

    /// <summary>
    /// Generates the specified number of new ballots.
    /// </summary>
    /// <param name="election">Election</param>
    /// <param name="numberOfBallots">Number of ballots to be generated</param>
    /// <returns></returns>
    Task CreateBallots(Election election, int numberOfBallots);

    /// <summary>
    /// Publishes the evidence of a cast ballot.
    /// </summary>
    /// <param name="election">Election</param>
    /// <param name="ballotId">Ballot identifier</param>
    /// <param name="spoiltBallotIndex">Ballot to be spoilt</param>
    /// <param name="selectedOptions">Selected short codes on the cast ballot.</param>
    /// <returns></returns>
    Task PublishBallotEvidence(Election election, string ballotId, int spoiltBallotIndex, IList<string> selectedOptions);
}