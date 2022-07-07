using AutoMapper;
using Helverify.VotingAuthority.Application.Services;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    /// <summary>
    /// Controller for handling the election API
    /// </summary>
    [Route("api/elections")]
    [ApiController]
    public class ElectionsController : ControllerBase
    {
        private const string ContentType = "application/json";

        private readonly IMapper _mapper;
        private readonly IElectionService _electionService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="electionService">Facade for election operations</param>
        /// <param name="mapper">Automapper</param>
        public ElectionsController(IElectionService electionService,
            IMapper mapper)
        {
            _electionService = electionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new election.
        /// </summary>
        /// <param name="electionDto">Election parameters</param>
        /// <returns>Newly created election</returns>
        [HttpPost]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<ElectionDto>> Post([FromBody] ElectionDto electionDto)
        {
            Election election = _mapper.Map<Election>(electionDto);

            election = await _electionService.CreateAsync(election);

            ElectionDto result = _mapper.Map<ElectionDto>(election);

            return Ok(result);
        }

        /// <summary>
        /// Provides the election with the specified id.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <returns>Election with the specified id.</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces(ContentType)]
        public async Task<ActionResult<ElectionDto>> Get(string id)
        {
            Election election = await _electionService.GetAsync(id);

            ElectionDto result = _mapper.Map<ElectionDto>(election);

            return Ok(result);
        }

        /// <summary>
        /// Provides a list of all elections.
        /// </summary>
        /// <returns>List of elections</returns>
        [HttpGet]
        [Produces(ContentType)]
        public async Task<ActionResult<IList<ElectionDto>>> Get()
        {
            IList<Election> elections = await _electionService.GetAsync();

            IList<ElectionDto> results = _mapper.Map<IList<ElectionDto>>(elections);

            return Ok(results);
        }

        /// <summary>
        /// Updates a specific election.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <param name="electionDto">Election parameters</param>
        /// <returns>Updated election</returns>
        [HttpPut]
        [Route("{id}")]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<Election>> Put(string id, [FromBody] ElectionDto electionDto)
        {
            Election election = _mapper.Map<Election>(electionDto);

            election = await _electionService.UpdateAsync(id, election);

            ElectionDto result = _mapper.Map<ElectionDto>(election);

            return Ok(result);
        }

        /// <summary>
        /// Removes an election.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            await _electionService.DeleteAsync(id);
        }

        /// <summary>
        /// Combines and stores the public keys of all registered consensus nodes of the specified election.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <returns>Election public key</returns>
        [HttpPut]
        [Route("{id}/public-key")]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<ElectionDto>> Put(string id)
        {
            Election election = await _electionService.GeneratePublicKeyAsync(id);

            ElectionDto result = _mapper.Map<ElectionDto>(election);

            return Ok(result);
        }


        /// <summary>
        /// Deploys a smart contract for the specified election.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/contract")]
        public async Task<ActionResult<ElectionDto>> DeployContract([FromRoute] string id)
        {
            Election election = await _electionService.DeployElectionContractAsync(id);

            ElectionDto electionDto = _mapper.Map<ElectionDto>(election);

            return Ok(electionDto);
        }

        /// <summary>
        /// Calculates the final tally and publishes the results with evidence.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/tally")]
        public async Task<ActionResult> CalculateTally([FromRoute] string id)
        {
            IList<DecryptedValue> decryptedValues = await _electionService.CalculateTallyAsync(id);

            return Ok(decryptedValues.Select(r => r.PlainText));
        }

        /// <summary>
        /// Returns the final tally.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/results")]
        public async Task<ActionResult<ElectionResultsDto>> GetResults([FromRoute] string id)
        {
            ElectionResults results = await _electionService.GetResultsAsync(id);

            ElectionResultsDto electionResultsDto = _mapper.Map<ElectionResultsDto>(results);

            return Ok(electionResultsDto);
        }
    }
}
