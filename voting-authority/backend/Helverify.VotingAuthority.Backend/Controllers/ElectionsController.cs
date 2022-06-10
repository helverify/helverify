using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    [Route("api/elections")]
    [ApiController]
    public class ElectionsController : ControllerBase
    {
        private readonly IMongoService<Election> _electionService;

        public ElectionsController(IMongoService<Election> electionService)
        {
            _electionService = electionService;
        }

        [HttpPost]
        public async Task<ActionResult<Election>> Post([FromBody] Election election)
        {
            await _electionService.CreateAsync(election);

            return Ok(election);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Election>> Get(string id)
        {
            return await _electionService.GetAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult<List<Election>>> Get()
        {
            return await _electionService.GetAsync();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Election>> Put(string id, Election election)
        {
            await _electionService.UpdateAsync(id, election);

            return election;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            await _electionService.RemoveAsync(id);
        }
    }
}
