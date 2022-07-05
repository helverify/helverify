using System.Collections.Concurrent;
using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.AspNetCore.Mvc;

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
        
        private readonly IRepository<Election> _electionRepository;
        private readonly IRepository<PaperBallot> _ballotRepository;
        private readonly IRepository<Blockchain> _bcRepository;
        private readonly IConsensusNodeService _consensusNodeService;
        private readonly IElectionContractRepository _contractRepository;
        private readonly IPublishedBallotRepository _publishedBallotRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="electionRepository">Provides access to elections stored on the database.</param>
        /// <param name="ballotRepository">Provides access to plain text ballots stored on the database.</param>
        /// <param name="publishedBallotRepository"></param>
        /// <param name="mapper">Automapper</param>
        /// <param name="consensusNodeService"></param>
        /// <param name="contractRepository">Provides access to the Election smart contract.</param>
        /// <param name="bcRepository">Provides access to the blockchain settings</param>
        public BallotsController(
            IRepository<Election> electionRepository,
            IRepository<PaperBallot> ballotRepository,
            IRepository<Blockchain> bcRepository,
            IConsensusNodeService consensusNodeService,
            IElectionContractRepository contractRepository,
            IPublishedBallotRepository publishedBallotRepository,
            IMapper mapper)
        {
            _electionRepository = electionRepository;
            _ballotRepository = ballotRepository;
            _bcRepository = bcRepository;
            _consensusNodeService = consensusNodeService;
            _mapper = mapper;
            _contractRepository = contractRepository;
            _publishedBallotRepository = publishedBallotRepository;
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
        public async Task<ActionResult> GenerateBallots([FromRoute] string electionId, [FromBody] BallotGenerationDto ballotParameters)
        {
            Election election = await _electionRepository.GetAsync(electionId);

            BallotTemplate ballotTemplate = election.GenerateBallotTemplate();

            ConcurrentQueue<PaperBallot> paperBallots = new ConcurrentQueue<PaperBallot>();

            Parallel.For(0, ballotParameters.NumberOfBallots, (_) =>
            {
                VirtualBallot ballot1 = ballotTemplate.Encrypt();
                VirtualBallot ballot2 = ballotTemplate.Encrypt();

                PaperBallot paperBallot = new PaperBallot(election, ballot1, ballot2);

                _publishedBallotRepository.StoreVirtualBallot(ballot1);
                _publishedBallotRepository.StoreVirtualBallot(ballot2);

                paperBallots.Enqueue(paperBallot);
            });

            await (_ballotRepository as PaperBallotRepository)!.InsertMany(paperBallots.ToArray());

            int partitionSize = 60;

            for (int i = 0; i < paperBallots.Count; i += partitionSize)
            {
                IList<PaperBallot> partition = paperBallots.Skip(i).Take(partitionSize).ToList();

                await _contractRepository.StoreBallotsAsync(election, partition);
            }

            return Ok(paperBallots.Count);
        }

        /// <summary>
        /// Publishes the evidence of a casted ballot, consisting of the selected short codes and the ballot to be spoiled.
        /// </summary>
        /// <param name="electionId">Election identifier</param>
        /// <param name="ballotId">Ballot identifier</param>
        /// <param name="evidenceDto">Evidence parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{ballotId}/evidence")]
        public async Task<IActionResult> PublishEvidence([FromRoute] string electionId, [FromRoute] string ballotId,
            [FromBody] EvidenceDto evidenceDto)
        {
            int spoiltBallotIndex = evidenceDto.SpoiltBallotIndex;

            Election election = await _electionRepository.GetAsync(electionId);

            PaperBallot paperBallot = await _ballotRepository.GetAsync(ballotId);

            paperBallot.Election = election;

            PublishedBallot ballot = (await _contractRepository.GetBallotAsync(election, ballotId))[1-spoiltBallotIndex];

            try
            {
                await PublishSelections(paperBallot, evidenceDto, ballot.BallotCode);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            VirtualBallot virtualBallot = await DecryptBallot(election, ballotId, spoiltBallotIndex);

            string cid = _publishedBallotRepository.StoreSpoiltBallot(virtualBallot, paperBallot.GetRandomness(spoiltBallotIndex));

            await _contractRepository.SpoilBallotAsync(paperBallot.BallotId, virtualBallot.Code, election, cid);

            return Ok();
        }

        private async Task<VirtualBallot> DecryptBallot(Election election, string ballotId, int spoiltBallotIndex)
        {
            PublishedBallot publishedBallot = (await _contractRepository.GetBallotAsync(election, ballotId))[spoiltBallotIndex];

            VirtualBallot virtualBallot = _publishedBallotRepository.RetrieveVirtualBallot(publishedBallot.IpfsCid);

            virtualBallot = await CoopDecryptBallot(virtualBallot, election.Id!, publishedBallot.IpfsCid);

            return virtualBallot;
        }

        private async Task PublishSelections(PaperBallot paperBallot, EvidenceDto evidenceDto, string ballotCode)
        {
            IList<string> selection = evidenceDto.SelectedOptions.OrderBy(s => s).ToList();
            
            if (!paperBallot.HasShortCodes(selection))
            {
                throw new ArgumentNullException(nameof(evidenceDto.SelectedOptions), "Selection is not valid");
            }
            
            await _contractRepository.PublishBallotSelectionAsync(paperBallot.Election, paperBallot.BallotId, ballotCode, selection);
        }
        
        private async Task<VirtualBallot> CoopDecryptBallot(VirtualBallot ballot, string electionId, string ipfsCid)
        {
            Election election = await _electionRepository.GetAsync(electionId);

            election.Blockchain = await _bcRepository.GetAsync(election.Blockchain.Id);

            List<OptionShare> optionShares = await GetOptionShares(election, ballot, ipfsCid);

            BallotShares ballotShares = new BallotShares(optionShares);

            ballot = ballotShares.CombineShares(election, ballot);

            return ballot;
        }

        private async Task<List<OptionShare>> GetOptionShares(Election election, VirtualBallot ballot, string ipfsCid)
        {
            List<OptionShare> optionShares = new List<OptionShare>();

            foreach (Registration consensusNode in election.Blockchain.Registrations)
            {
                DecryptedBallotShareDto? decryptedBallot =
                    await _consensusNodeService.DecryptBallotAsync(consensusNode.Endpoint, ballot, election.Id!, ipfsCid);

                if (decryptedBallot == null)
                {
                    throw new NullReferenceException("Decrypted ballot share must not be null.");
                }

                decryptedBallot.PublicKey = consensusNode.PublicKeys[election.Id!];

                IList<OptionShare> optionSharesPart = _mapper.Map<IList<OptionShare>>(decryptedBallot);

                optionShares.AddRange(optionSharesPart);
            }

            return optionShares;
        }
    }
}
