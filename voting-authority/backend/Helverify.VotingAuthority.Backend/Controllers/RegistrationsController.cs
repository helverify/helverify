using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.DataAccess.Rest;
using Helverify.VotingAuthority.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    [Route("api/elections/{electionId}/registrations")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IMongoService<Registration> _registrationService;
        private readonly IMongoService<Election> _electionService;
        private readonly IRestClient _restClient;

        public RegistrationsController(IMongoService<Registration> registrationService, IMongoService<Election> electionService, IRestClient restClient)
        {
            _registrationService = registrationService;
            _electionService = electionService;
            _restClient = restClient;
        }

        [HttpPost]
        public async Task<ActionResult<Registration>> Post([FromRoute] string electionId, [FromBody] Registration registration)
        {
            registration.ElectionId = electionId;
            
            Election election = await _electionService.GetAsync(electionId);

            PublicKeyDto publicKey = await _restClient.Call<PublicKeyDto>(HttpMethod.Post, new Uri(registration.Endpoint, "/api/key-pair"),
                new KeyPairRequestDto { P = election.P, G = election.G });

            // Todo: proof validation

            registration.PublicKey = publicKey.PublicKey;

            await _registrationService.CreateAsync(registration);

            return registration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Registration>>> Get()
        {
            return await _registrationService.GetAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Registration>> Get(string id)
        {
            return await _registrationService.GetAsync(id);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Registration>> Put(string id, Registration registration)
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
