using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Ipfs;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Helverify.VotingAuthority.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    [Route("api/elections/{electionId}/ballots")]
    [ApiController]
    public class BallotsController : ControllerBase
    {
        private const string ContentType = "application/json";

        private readonly IStorageClient _storageClient;
        private readonly IRepository<Election> _electionRepository;
        //private readonly IRepository<PaperBallot> _ballotRepository;
        private readonly IMapper _mapper;
        private readonly IElectionContractRepository _contractRepository;

        public BallotsController(IStorageClient storageClient, 
            IRepository<Election> electionRepository, 
            //IRepository<PaperBallot> ballotRepository, 
            IMapper mapper, 
            IElectionContractRepository contractRepository)
        {
            _storageClient = storageClient;
            _electionRepository = electionRepository;
            //_ballotRepository = ballotRepository;
            _mapper = mapper;
            _contractRepository = contractRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PaperBallotDao>> Retrieve()
        {
            PaperBallotDao paperBallot = await _storageClient.Retrieve<PaperBallotDao>("QmSn9QMnPyXnhUSxU8W6BQJ9gSeXj7AdyBWXHVP48u22tL");

            return paperBallot;
        }

        [HttpPost]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult> CreateBallots([FromRoute] string electionId, [FromBody] BallotGenerationDto ballotParameters)
        {
            Election election = await _electionRepository.GetAsync(electionId);
            
            BallotTemplate ballotTemplate = election.GenerateBallotTemplate();

            for (int i = 0; i < ballotParameters.NumberOfBallots; i++)
            {
                VirtualBallot ballot1 = CreateVirtualBallot(ballotTemplate);
                VirtualBallot ballot2 = CreateVirtualBallot(ballotTemplate);

                PaperBallot paperBallot = new PaperBallot(election, ballot1, ballot2);

                VirtualBallotDao ballot1Dao = _mapper.Map<VirtualBallotDao>(ballot1);
                VirtualBallotDao ballot2Dao = _mapper.Map<VirtualBallotDao>(ballot2);

                string cid1 = await _storageClient.Store(ballot1Dao);
                string cid2 = await _storageClient.Store(ballot2Dao);

                await _contractRepository.StoreBallot(election, paperBallot.BallotId, ballot1.Code, cid1, ballot2.Code, cid2);
                
                //await _ballotRepository.CreateAsync(paperBallot); TODO: define mappings
            }

            

            IList<string> ballotIds = await _contractRepository.GetBallotIds(election);

            return Ok(ballotIds);
        }

        private VirtualBallot CreateVirtualBallot(BallotTemplate ballotTemplate)
        {
            VirtualBallot virtualBallot = ballotTemplate.Encrypt();

            while (!virtualBallot.AreShortCodesUnique())
            {
                virtualBallot = ballotTemplate.Encrypt();
            }

            return virtualBallot;
        }
    }
}
