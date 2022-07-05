using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;

namespace Helverify.VotingAuthority.Application.Services
{
    /// <inheritdoc cref="IBallotPrintService"/>
    internal class BallotPrintService : IBallotPrintService
    {
        private readonly IRepository<PaperBallot> _ballotRepository;
        private readonly IBallotPdfService _pdfService;
        private readonly IZipFileService _zipFileService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ballotRepository"></param>
        /// <param name="pdfService"></param>
        /// <param name="zipFileService"></param>
        public BallotPrintService(IRepository<PaperBallot> ballotRepository,
            IBallotPdfService pdfService,
            IZipFileService zipFileService)
        {
            _ballotRepository = ballotRepository;
            _pdfService = pdfService;
            _zipFileService = zipFileService;
        }

        /// <inheritdoc cref="IBallotPrintService.GeneratePdfsZipped"/>
        public async Task<byte[]> GeneratePdfsZipped(Election election, IList<PaperBallot> paperBallots)
        {
            IList<ArchiveFile> archiveFiles = await _pdfService.GeneratePdfs(election, paperBallots);

            foreach (PaperBallot paperBallot in paperBallots)
            {
                await _ballotRepository.UpdateAsync(paperBallot.BallotId, paperBallot);
            }

            byte[] zipFile = _zipFileService.CreateZip(archiveFiles);

            return zipFile;
        }
    }
}
