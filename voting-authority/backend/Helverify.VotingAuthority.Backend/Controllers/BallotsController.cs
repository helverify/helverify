using System.Collections.Concurrent;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using AutoMapper;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Backend.Template;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.DataAccess.Ipfs;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math;
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
        private readonly IRepository<Blockchain> _bcRepository;
        private readonly IConsensusNodeService _consensusNodeService;
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
            IRepository<Blockchain> bcRepository,
            IConsensusNodeService consensusNodeService,
            IElectionContractRepository contractRepository,
            IMapper mapper)
        {
            _storageClient = storageClient;
            _electionRepository = electionRepository;
            _ballotRepository = ballotRepository;
            _bcRepository = bcRepository;
            _consensusNodeService = consensusNodeService;
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

                DataAccess.Ethereum.Contract.PaperBallot pb = new DataAccess.Ethereum.Contract.PaperBallot
                {
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
        public async Task<ActionResult> GeneratePdf([FromRoute] string electionId, [FromQuery] string ballotId)
        {
            Election election = await _electionRepository.GetAsync(electionId);

            PaperBallot paperBallot = await _ballotRepository.GetAsync(ballotId);

            IDocument paperBallotTemplate = new PaperBallotTemplate(election, paperBallot);

            byte[] pdfBytes = paperBallotTemplate.GeneratePdf();

            Stream stream = new MemoryStream(pdfBytes);

            return File(stream, ContentTypePdf, $"{ballotId}{FileExtensionPdf}");
        }


        [HttpGet]
        [Route("pdf/all")]
        public async Task<ActionResult> GenerateAllPdfs([FromRoute] string electionId, int numberOfBallots)
        {
            Election election = await _electionRepository.GetAsync(electionId);

            IList<PaperBallot> paperBallots = (await _ballotRepository.GetAsync()).Where(b => !b.Printed).Take(numberOfBallots).ToList();

            if (!paperBallots.Any())
            {
                return NoContent();
            }

            // inspired by https://stackoverflow.com/questions/51740673/building-a-corrupted-zip-file-using-asp-net-core-and-angular-6
            await using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zipFile = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (PaperBallot paperBallot in paperBallots)
                    {
                        IDocument paperBallotTemplate = new PaperBallotTemplate(election, paperBallot);

                        byte[] pdfBytes = paperBallotTemplate.GeneratePdf();

                        ZipArchiveEntry archiveEntry = zipFile.CreateEntry($"{paperBallot.BallotId}.pdf");

                        await using (Stream fileStream = new MemoryStream(pdfBytes))
                        await using (Stream entryStream = archiveEntry.Open())
                        {
                            fileStream.CopyTo(entryStream);
                        }

                        paperBallot.Printed = true;

                        paperBallot.ClearConfidential();

                        await _ballotRepository.UpdateAsync(paperBallot.BallotId, paperBallot);
                    }
                }

                return File(zipStream.ToArray(), "application/zip", $"ballots_{election.Id}.zip");
            }
        }

        /// <summary>
        /// Publishes the evidence of a casted ballot, consisting of the selected short codes and the ballot to be spoiled.
        /// </summary>
        /// <param name="electionId"></param>
        /// <param name="id"></param>
        /// <param name="evidenceDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/evidence")]
        public async Task<IActionResult> Post([FromRoute] string electionId, [FromRoute] string id,
            [FromBody] EvidenceDto evidenceDto)
        {
            int spoiltBallotIndex = evidenceDto.SpoiltBallotIndex;

            if (spoiltBallotIndex != 0 && spoiltBallotIndex != 1)
            {
                return BadRequest("Ballot index to be spoilt is invalid (allowed values: 0 or 1)");
            }

            Election election = await _electionRepository.GetAsync(electionId);

            IList<string> selection = evidenceDto.SelectedOptions;

            selection = selection.OrderBy(s => s).ToList();

            PaperBallot paperBallot = await _ballotRepository.GetAsync(id);

            if (!paperBallot.HasShortCodes(selection))
            {
                return BadRequest("Selection does not exist on this ballot. Please check that you supplied valid short codes.");
            }
            
            // Publish short codes of the selection options on the smart contract
            await _contractRepository.PublishShortCodes(election, id, selection);

            // decrypt spoilt ballot

            DataAccess.Ethereum.Contract.PaperBallot paperBallotDto = await _contractRepository.GetBallot(election, id);

            string ipfsCid = string.Empty;
            
            IDictionary<string, IList<BigInteger>> randomness = new Dictionary<string, IList<BigInteger>>();


            if (spoiltBallotIndex == 0)
            {
                ipfsCid = paperBallotDto.Ballot1Ipfs;

                foreach (PaperBallotOption ballotOption in paperBallot.Options)
                {
                    randomness[ballotOption.ShortCode1] = ballotOption.RandomValues1;
                }
            }

            if (spoiltBallotIndex == 1)
            {
                ipfsCid = paperBallotDto.Ballot2Ipfs;

                foreach (PaperBallotOption ballotOption in paperBallot.Options)
                {
                    randomness[ballotOption.ShortCode2] = ballotOption.RandomValues2;
                }
            }

            VirtualBallotDao encryption = await _storageClient.Retrieve<VirtualBallotDao>(ipfsCid);

            VirtualBallot virtualBallot = _mapper.Map<VirtualBallot>(encryption);

            virtualBallot = await DecryptBallot(virtualBallot, electionId, ipfsCid);

            // publish spoiled ballot on ipfs
            SpoiltBallotDao spoiltBallot = _mapper.Map<SpoiltBallotDao>(virtualBallot);

            spoiltBallot.SetRandomness(randomness);
            
            string cid = await _storageClient.Store(spoiltBallot);

            // call spoilBallot on contract
            await _contractRepository.SpoilBallot(id, virtualBallot.Code, election, cid);
            
            return Ok();
        }

        private async Task<VirtualBallot> DecryptBallot(VirtualBallot ballot, string electionId, string ipfsCid)
        {
            Election election = await _electionRepository.GetAsync(electionId);

            election.Blockchain = await _bcRepository.GetAsync(election.Blockchain.Id);

            IList<OptionShare> optionShares = new List<OptionShare>();

            foreach (Registration consensusNode in election.Blockchain.Registrations)
            {
                DecryptedBallotShareDto? decryptedBallot = await _consensusNodeService.DecryptBallot(consensusNode.Endpoint, ballot, electionId, ipfsCid);

                foreach (string key in decryptedBallot.DecryptedShares.Keys)
                {
                    IList<DecryptionShareDto> decryptedBallotDecryptedShare = decryptedBallot.DecryptedShares[key];

                    IList<DecryptedShare> shares = new List<DecryptedShare>();

                    foreach (DecryptionShareDto decryptedShare in decryptedBallotDecryptedShare)
                    {
                        DecryptedShare share = new DecryptedShare
                        {
                            Share = new BigInteger(decryptedShare.DecryptedShare, 16),
                            ProofOfDecryption = new ProofOfDecryption(new BigInteger(decryptedShare.ProofOfDecryption.D, 16),
                                new BigInteger(decryptedShare.ProofOfDecryption.U, 16),
                                new BigInteger(decryptedShare.ProofOfDecryption.V, 16),
                                new BigInteger(decryptedShare.ProofOfDecryption.S, 16)),
                            PublicKeyShare = consensusNode.PublicKeys[electionId]
                        };

                        shares.Add(share);
                    }

                    optionShares.Add(new OptionShare
                    {
                        ShortCode = key,
                        Shares = shares,
                    });
                }
            }

            BallotShares ballotShares = new BallotShares(optionShares);

            ballot = ballotShares.CombineShares(election, ballot);

            return ballot;
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
