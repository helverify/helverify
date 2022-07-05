using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Template;
using QuestPDF.Infrastructure;

namespace Helverify.VotingAuthority.Domain.Service
{
    /// <inheritdoc cref="IBallotPdfService"/>
    internal class BallotPdfService : IBallotPdfService
    {
        private const string FileExtensionPdf = ".pdf";

        /// <inheritdoc cref="IBallotPdfService.GeneratePdfs"/>
        public async Task<IList<ArchiveFile>> GeneratePdfs(Election election, IList<PaperBallot> ballots)
        {
            IList<ArchiveFile> archiveFiles = new List<ArchiveFile>();

            foreach (PaperBallot paperBallot in ballots)
            {
                IDocument paperBallotTemplate = new PaperBallotTemplate(election, paperBallot);

                byte[] pdfBytes = paperBallot.CreatePdf(paperBallotTemplate);

                archiveFiles.Add(new ArchiveFile($"{paperBallot.BallotId}{FileExtensionPdf}", pdfBytes));
            }

            return archiveFiles;
        }
    }
}
