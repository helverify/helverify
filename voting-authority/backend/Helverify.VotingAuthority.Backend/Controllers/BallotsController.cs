using System.Collections.Concurrent;
using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Backend.Template;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Ipfs;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Helverify.VotingAuthority.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

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
        private const string ContentTypePdf = "application/pdf";
        private const string FileExtensionPdf = ".pdf";

        private readonly IStorageClient _storageClient;
        private readonly IRepository<Election> _electionRepository;
        private readonly IRepository<PaperBallot> _ballotRepository;
        private readonly IMapper _mapper;
        private readonly IElectionContractRepository _contractRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storageClient">Provides access to IFPS</param>
        /// <param name="electionRepository">Provides access to elections stored on the database.</param>
        /// <param name="ballotRepository">Provides access to plain text ballots stored on the database.</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="contractRepository">Provides access to the Election smart contract.</param>
        public BallotsController(IStorageClient storageClient, 
            IRepository<Election> electionRepository, 
            IRepository<PaperBallot> ballotRepository, 
            IMapper mapper, 
            IElectionContractRepository contractRepository)
        {
            _storageClient = storageClient;
            _electionRepository = electionRepository;
            _ballotRepository = ballotRepository;
            _mapper = mapper;
            _contractRepository = contractRepository;
        }

        /// <summary>
        /// TODO: refactor, only temporary for demo purposes
        /// </summary>
        /// <param name="electionId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Consumes(ContentTypeJson)]
        [Produces(ContentTypeJson)]
        [Route("encrypted")]
        public async Task<ActionResult<IList<VirtualBallotDao>>> Get([FromRoute] string electionId, string id)
        {
            Election election = await _electionRepository.GetAsync(electionId);

            DataAccess.Ethereum.Contract.PaperBallot paperBallotDto = await _contractRepository.GetBallot(election, id);

            VirtualBallotDao paperBallot1 = await _storageClient.Retrieve<VirtualBallotDao>(paperBallotDto.Ballot1Ipfs);
            VirtualBallotDao paperBallot2 = await _storageClient.Retrieve<VirtualBallotDao>(paperBallotDto.Ballot2Ipfs);

            return new List<VirtualBallotDao> { paperBallot1, paperBallot2 };
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
            PaperBallot paperBallot = await _ballotRepository.GetAsync(id);

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
        public async Task<ActionResult> Post([FromRoute] string electionId, [FromBody] BallotGenerationDto ballotParameters)
        {
            Election election = await _electionRepository.GetAsync(electionId);
            
            BallotTemplate ballotTemplate = election.GenerateBallotTemplate();
            
            ConcurrentQueue<DataAccess.Ethereum.Contract.PaperBallot> pbs = new ConcurrentQueue<DataAccess.Ethereum.Contract.PaperBallot>();
            ConcurrentQueue<PaperBallot> paperBallots = new ConcurrentQueue<PaperBallot>();

            Parallel.For(0, ballotParameters.NumberOfBallots, (_) =>
            {
            
                VirtualBallot ballot1 = CreateVirtualBallot(ballotTemplate);
                VirtualBallot ballot2 = CreateVirtualBallot(ballotTemplate);

                PaperBallot paperBallot = new PaperBallot(election, ballot1, ballot2);

                VirtualBallotDao ballot1Dao = _mapper.Map<VirtualBallotDao>(ballot1);
                VirtualBallotDao ballot2Dao = _mapper.Map<VirtualBallotDao>(ballot2);

                string cid1 = _storageClient.Store(ballot1Dao).Result;
                string cid2 = _storageClient.Store(ballot2Dao).Result;

                DataAccess.Ethereum.Contract.PaperBallot pb = new DataAccess.Ethereum.Contract.PaperBallot{
                    Ballot1Code = ballot1.Code,
                    Ballot1Ipfs = cid1,
                    Ballot2Code = ballot2.Code,
                    Ballot2Ipfs = cid2,
                    BallotId = paperBallot.BallotId
                };

                paperBallots.Enqueue(paperBallot);
                pbs.Enqueue(pb);
            });
            
            await (_ballotRepository as PaperBallotRepository)!.InsertMany(paperBallots.ToArray());

            int partitionSize = 60;

            for (int i = 0; i < pbs.Count; i += partitionSize)
            {
                IList<DataAccess.Ethereum.Contract.PaperBallot> partition = pbs.Skip(i).Take(partitionSize).ToList();

                await _contractRepository.StoreBallots(election, partition);
            }

            return Ok(paperBallots.Count);
        }

        /// <summary>
        /// Generates a PDF for the specified ballot.
        /// </summary>
        /// <param name="electionId">Current election id</param>
        /// <param name="ballotId">Id of ballot to be printed</param>
        /// <returns></returns>
        [HttpGet]
        [Route("pdf")]
        [Produces(ContentTypePdf)]
        public async Task<ActionResult> GeneratePdf([FromRoute] string electionId, [FromQuery]string ballotId)
        {
            Election election = await _electionRepository.GetAsync(electionId);

            PaperBallot paperBallot = await _ballotRepository.GetAsync(ballotId);

            IDocument paperBallotTemplate = new PaperBallotTemplate(election, paperBallot);

            byte[] pdfBytes = paperBallotTemplate.GeneratePdf();
            
            Stream stream = new MemoryStream(pdfBytes);
            
            return File(stream, ContentTypePdf, $"{ballotId}{FileExtensionPdf}");
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
