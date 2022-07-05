using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;

namespace Helverify.VotingAuthority.Application.Services;

/// <summary>
/// Facade for ballot print domain logic.
/// </summary>
public interface IBallotPrintService
{
    /// <summary>
    /// Generates PDFs for the specified ballots and puts them into a ZIP file.
    /// </summary>
    /// <param name="election">Election</param>
    /// <param name="paperBallots">Paper ballots to be printed to PDF</param>
    /// <returns></returns>
    Task<byte[]> GeneratePdfsZipped(Election election, IList<PaperBallot> paperBallots);
}