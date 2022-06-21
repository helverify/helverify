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
        private readonly IpfsClient _ipfsClient;

        public BallotsController(IpfsClient ipfsClient)
        {
            _ipfsClient = ipfsClient;
        }

        [HttpGet]
        public async Task<ActionResult<PaperBallotDao>> Retrieve()
        {
            StorageClient storageClient = new StorageClient(_ipfsClient);

            PaperBallotDao paperBallot = await storageClient.Retrieve<PaperBallotDao>("QmSn9QMnPyXnhUSxU8W6BQJ9gSeXj7AdyBWXHVP48u22tL");

            return paperBallot;

        }
    }
}
