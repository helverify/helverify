using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    [Route("api/elections/{electionId}/registrations")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IMongoService<Registration> _registrationService;

        public RegistrationsController(IMongoService<Registration> registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost]
        public async Task<ActionResult<Registration>> Post([FromRoute] string electionId, [FromBody] Registration registration)
        {
            registration.ElectionId = electionId;

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
