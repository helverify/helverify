using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    [Route("api/elections/{electionId}/registrations")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private const string ContentType = "application/json";

        private readonly IMongoService<Registration> _registrationService;
        private readonly IMongoService<Election> _electionService;
        private readonly IConsensusNodeService _consensusNodeService;
        
        public RegistrationsController(IMongoService<Registration> registrationService, IMongoService<Election> electionService, IConsensusNodeService consensusNodeService)
        {
            _registrationService = registrationService;
            _electionService = electionService;
            _consensusNodeService = consensusNodeService;
        }

        [HttpPost]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<Registration>> Post([FromRoute] string electionId, [FromBody] Registration registration)
        {
            registration.ElectionId = electionId;
            
            Election election = await _electionService.GetAsync(electionId);

            PublicKeyDto publicKey = await _consensusNodeService.GenerateKeyPairAsync(registration.Endpoint, election);

            registration.SetPublicKey(publicKey, election);
           
            await _registrationService.CreateAsync(registration);

            return registration;
        }

        [HttpGet]
        [Produces(ContentType)]
        public async Task<ActionResult<List<Registration>>> Get()
        {
            return await _registrationService.GetAsync();
        }

        [HttpGet]
        [Route("{id}")]
        [Produces(ContentType)]
        public async Task<ActionResult<Registration>> Get(string id)
        {
            return await _registrationService.GetAsync(id);
        }

        [HttpPut]
        [Route("{id}")]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<Registration>> Put(string id, [FromBody] Registration registration)
        {
            await _registrationService.UpdateAsync(id, registration);

            return registration;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            await _registrationService.RemoveAsync(id);
        }
    }
}
