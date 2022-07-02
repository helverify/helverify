using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Template;
using QuestPDF.Infrastructure;

namespace Helverify.VotingAuthority.Domain.Service
{
    internal class BallotPdfService : IBallotPdfService
    {
        private const string FileExtensionPdf = ".pdf";
        
        public async Task<IList<ArchiveFile>> GeneratePdfs(Election election, IList<PaperBallot> ballots)
        {
            IList<ArchiveFile> archiveFiles = new List<ArchiveFile>();

            foreach (PaperBallot paperBallot in ballots)
            {
                IDocument paperBallotTemplate = new PaperBallotTemplate(election, paperBallot);

                byte[] pdfBytes = paperBallot.CreatePdf(paperBallotTemplate);

                archiveFiles.Add(new ArchiveFile
                {
                    FileName = $"{paperBallot.BallotId}{FileExtensionPdf}",
                    Data = pdfBytes
                });
            }

            return archiveFiles;
        }
    }
}
