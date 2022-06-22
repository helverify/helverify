using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Ipfs;
using Ipfs.Http;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BallotsController : ControllerBase
    {
        private readonly IStorageClient _storageClient;

        public BallotsController(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        [HttpGet]
        public async Task<ActionResult<PaperBallotDao>> Retrieve()
        {
            PaperBallotDao paperBallot = await _storageClient.Retrieve<PaperBallotDao>("QmSn9QMnPyXnhUSxU8W6BQJ9gSeXj7AdyBWXHVP48u22tL");

            return paperBallot;

        }
    }
}
