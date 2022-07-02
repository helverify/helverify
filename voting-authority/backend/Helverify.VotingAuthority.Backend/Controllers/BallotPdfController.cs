using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    /// <summary>
    /// Controller for Ballot PDF handling
    /// </summary>
    [Route("api/elections/{electionId}/ballots/pdf")]
    [ApiController]
    public class BallotPdfController: ControllerBase
    {
        private const string FileExtensionZip = ".zip";
        private const string ContentTypeZip = "application/zip";

        private readonly IRepository<Election> _electionRepository;
        private readonly IRepository<PaperBallot> _ballotRepository;
        private readonly IBallotPdfService _pdfService;
        private readonly IZipFileService _zipFileService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="electionRepository">Data access to elections</param>
        /// <param name="ballotRepository">Data access to ballots</param>
        /// <param name="pdfService">PDF generator</param>
        /// <param name="zipFileService">ZIP file generator</param>
        public BallotPdfController(IRepository<Election> electionRepository,
            IRepository<PaperBallot> ballotRepository,
            IBallotPdfService pdfService,
            IZipFileService zipFileService)
        {
            _electionRepository = electionRepository;
            _ballotRepository = ballotRepository;
            _pdfService = pdfService;
            _zipFileService = zipFileService;
        }

        /// <summary>
        /// Generates PDF ballots packed into a ZIP file.
        /// </summary>
        /// <param name="electionId">Election identifier</param>
        /// <param name="numberOfBallots">Number of ballots to be printed</param>
        /// <returns></returns>
        [HttpGet]
        [Produces(ContentTypeZip)]
        public async Task<ActionResult> GenerateAllPdfs([FromRoute] string electionId, int numberOfBallots)
        {
            Election election = await _electionRepository.GetAsync(electionId);

            IList<PaperBallot> paperBallots = (await _ballotRepository.GetAsync()).Where(b => !b.Printed).Take(numberOfBallots).ToList();

            if (!paperBallots.Any())
            {
                return NoContent();
            }

            IList<ArchiveFile> archiveFiles = await _pdfService.GeneratePdfs(election, paperBallots);

            foreach (PaperBallot paperBallot in paperBallots)
            {
                await _ballotRepository.UpdateAsync(paperBallot.BallotId, paperBallot);
            }

            byte[] zipFile = _zipFileService.CreateZip(archiveFiles);

            return File(zipFile, ContentTypeZip, $"ballots_{election.Id}{FileExtensionZip}");
        }
    }
}
