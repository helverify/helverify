using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;

namespace Helverify.VotingAuthority.Domain.Service;

/// <summary>
/// Provides PDF functionality for Paper Ballots
/// </summary>
public interface IBallotPdfService
{
    /// <summary>
    /// Generates PDFs from Paper Ballots
    /// </summary>
    /// <param name="election">Current election</param>
    /// <param name="ballots">Paper ballots to be printed</param>
    /// <returns></returns>
    Task<IList<ArchiveFile>> GeneratePdfs(Election election, IList<PaperBallot> ballots);
}