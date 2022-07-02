using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;

namespace Helverify.VotingAuthority.Domain.Service;

public interface IBallotPdfService
{
    Task<IList<ArchiveFile>> GeneratePdfs(Election election, IList<PaperBallot> ballots);
}