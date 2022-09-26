using AutoMapper;
using Helverify.VotingAuthority.Application.Services;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    /// <summary>
    /// Controller for handling ballot life-cycle.
    /// </summary>
    [Route("api/elections/{electionId}/ballots")]
    [ApiController]
    public class BallotsController : ControllerBase
    {
        private const string ContentTypeJson = "application/json";

        private readonly IBallotService _ballotService;
        private readonly IElectionService _electionService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ballotService">Facade for ballot domain logic</param>
        /// <param name="electionService">Facade for election domain logic</param>
        /// <param name="mapper">Automapper</param>
        public BallotsController(
            IBallotService ballotService,
            IElectionService electionService,
            IMapper mapper)
        {
            _ballotService = ballotService;
            _electionService = electionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Shows the ballot data needed for printing a paper ballot.
        /// </summary>
        /// <param name="id">Ballot id</param>
        /// <returns></returns>
        [HttpGet]
        [Produces(ContentTypeJson)]
        public async Task<ActionResult<PrintBallotDto>> GetPrint(string id)
        {
            PaperBallot paperBallot = await _ballotService.GetAsync(id);

            PrintBallotDto printBallot = _mapper.Map<PrintBallotDto>(paperBallot);

            return printBallot;
        }

        /// <summary>
        /// Generates new ballots, stores the encryptions on IPFS, publishes the evidence and the IPFS CIDs on the smart contract,
        /// and persists the plaintext print ballots onto the database.
        /// </summary>
        /// <param name="electionId">Election ID</param>
        /// <param name="ballotParameters">Contains the ballot generation parameters, such as number of ballots to be created.</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(ContentTypeJson)]
        [Produces(ContentTypeJson)]
        public async Task<ActionResult> GenerateBallots([FromRoute] string electionId, [FromBody] BallotGenerationDto ballotParameters)
        {
            Election election = await _electionService.GetAsync(electionId);

            await _ballotService.CreateBallots(election, ballotParameters.NumberOfBallots);

            return Ok();
        }

        /// <summary>
        /// Publishes the evidence of a casted ballot, consisting of the selected short codes and the ballot to be spoiled.
        /// </summary>
        /// <param name="electionId">Election identifier</param>
        /// <param name="ballotId">Ballot identifier</param>
        /// <param name="evidenceDto">Evidence parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{ballotId}/evidence")]
        public async Task<IActionResult> PublishEvidence([FromRoute] string electionId, [FromRoute] string ballotId,
            [FromBody] EvidenceDto evidenceDto)
        {
            int spoiltBallotIndex = evidenceDto.SpoiltBallotIndex;
            
            IList<string> selections = evidenceDto.SelectedOptions;

            Election election = await _electionService.GetAsync(electionId);

            await _ballotService.PublishBallotEvidence(election, ballotId, spoiltBallotIndex, selections);

            return Ok();
        }

        [HttpPost]
        [Route("{ballotId}/evidence/evaluation")]
        public async Task<IActionResult> PublishRandomSelections([FromRoute] string electionId)
        {
            Election election = await _electionService.GetAsync(electionId);
            
            await _ballotService.PublishRandomEvidence(election);

            return Ok();
        }
    }
}
