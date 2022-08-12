using Helverify.VotingAuthority.Application.Services;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
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

        private readonly IElectionService _electionService;
        private readonly IBallotPrintService _ballotPrintService;
        private readonly IBallotService _ballotService;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ballotService">Facade for ballot domain logic.</param>
        /// <param name="electionService">Facade for election domain logic.</param>
        /// <param name="ballotPrintService">Facade for ballot print domain logic.</param>
        public BallotPdfController(
            IElectionService electionService,
            IBallotPrintService ballotPrintService,
        IBallotService ballotService)
        {
            _electionService = electionService;
            _ballotPrintService = ballotPrintService;
            _ballotService = ballotService;
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
            Election election = await _electionService.GetAsync(electionId);

            IList<PaperBallot> paperBallots = await _ballotService.GetAsync(election, numberOfBallots);

            if (!paperBallots.Any())
            {
                return NoContent();
            }
            
            byte[] zipFile = await _ballotPrintService.GeneratePdfsZipped(election, paperBallots);

            return File(zipFile, ContentTypeZip, $"ballots_{election.Id}{FileExtensionZip}");
        }
    }
}
