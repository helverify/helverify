using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    /// <summary>
    /// Controller for handling the registration API.
    /// </summary>
    [Route("api/elections/{electionId}/registrations")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private const string ContentType = "application/json";

        private readonly IRepository<Registration> _registrationRepository;
        private readonly IRepository<Election> _electionRepository;
        private readonly IConsensusNodeService _consensusNodeService;
        private readonly IMapper _mapper;
        private readonly IBlockchainSetup _blockchainSetup;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="registrationRepository">Repository for registrations</param>
        /// <param name="electionRepository">Repository for elections</param>
        /// <param name="consensusNodeService">Accessor to consensus node service</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="blockchainSetup">Service to set up the blockchain</param>
        public RegistrationsController(IRepository<Registration> registrationRepository, IRepository<Election> electionRepository, 
            IConsensusNodeService consensusNodeService, IMapper mapper, IBlockchainSetup blockchainSetup)
        {
            _registrationRepository = registrationRepository;
            _electionRepository = electionRepository;
            _consensusNodeService = consensusNodeService;
            _mapper = mapper;
            _blockchainSetup = blockchainSetup;
        }

        /// <summary>
        /// Register a new consensus node.
        /// </summary>
        /// <param name="electionId">Election identifier</param>
        /// <param name="registrationDto">Consensus node registration details</param>
        /// <returns>Newly created registration details</returns>
        [HttpPost]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<RegistrationDto>> Post([FromRoute] string electionId, [FromBody] RegistrationDto registrationDto)
        {
            Registration registration = _mapper.Map<Registration>(registrationDto);

            registration.ElectionId = electionId;
            
            Election election = await _electionRepository.GetAsync(electionId);

            PublicKeyDto? publicKey = await _consensusNodeService.GenerateKeyPairAsync(registrationDto.Endpoint, election);

            registration.SetPublicKey(publicKey, election);

            registration = await _registrationRepository.CreateAsync(registration);

            RegistrationDto result = _mapper.Map<RegistrationDto>(registration);

            return Ok(result);
        }

        /// <summary>
        /// Provides a list of all registered consensus nodes.
        /// </summary>
        /// <returns>List of registered consensus nodes</returns>
        [HttpGet]
        [Produces(ContentType)]
        public async Task<ActionResult<List<RegistrationDto>>> Get()
        {
            IList<Registration> registrations = await _registrationRepository.GetAsync();

            IList<RegistrationDto> results = _mapper.Map<IList<RegistrationDto>>(registrations);
            
            return Ok(results);
        }

        /// <summary>
        /// Provides the details of a specific consensus node registration.
        /// </summary>
        /// <param name="id">Registration identifier</param>
        /// <returns>Details of a registered consensus node.</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces(ContentType)]
        public async Task<ActionResult<RegistrationDto>> Get(string id)
        {
            Registration registration = await _registrationRepository.GetAsync(id);

            RegistrationDto result = _mapper.Map<RegistrationDto>(registration);
            
            return Ok(result);
        }

        /// <summary>
        /// Updates a consensus node registration.
        /// </summary>
        /// <param name="id">Registration identifier</param>
        /// <param name="registrationDto">Registration details</param>
        /// <returns>Updated consensus node registration</returns>
        [HttpPut]
        [Route("{id}")]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<RegistrationDto>> Put(string id, [FromBody] RegistrationDto registrationDto)
        {
            Registration registration = _mapper.Map<Registration>(registrationDto);

            registration = await _registrationRepository.UpdateAsync(id, registration);

            RegistrationDto result = _mapper.Map<RegistrationDto>(registration);
            
            return Ok(result);
        }

        /// <summary>
        /// Removes a consensus node registration.
        /// </summary>
        /// <param name="id">Registration identifier</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            await _registrationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Initializes the Proof-of-Authority blockchain using the consensus nodes registered for the election.
        /// </summary>
        /// <param name="electionId">Election identifier</param>
        /// <returns></returns>
        [HttpPost]
        [Route("blockchain-setup")]
        public async Task Setup([FromRoute] string electionId)
        {
            Election election = await _electionRepository.GetAsync(electionId);
            
            IList<Registration> registrations = election.Registrations;

            string nodeAddress = await _blockchainSetup.CreateAccountsAsync(registrations);

            Genesis genesis = await _blockchainSetup.PropagateGenesisBlockAsync(registrations, new Account(nodeAddress, "1000000000000000000000000000000000000000"));

            NodesDto nodes = await _blockchainSetup.StartPeersAsync(registrations);

            await _blockchainSetup.InitializeNodesAsync(registrations, nodes);

            await _blockchainSetup.StartSealingAsync(registrations);

            await UpdateRegistrations(registrations);

            _blockchainSetup.RegisterRpcEndpoint(genesis, nodes);
        }

        private async Task UpdateRegistrations(IList<Registration> registrations)
        {
            foreach (Registration registration in registrations)
            {
                await _registrationRepository.UpdateAsync(registration.Id!, registration);
            }
        }
    }
}
