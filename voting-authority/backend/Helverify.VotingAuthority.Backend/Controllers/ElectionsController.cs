using AutoMapper;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    /// <summary>
    /// Controller for handling the election API
    /// </summary>
    [Route("api/elections")]
    [ApiController]
    public class ElectionsController : ControllerBase
    {
        private const string ContentType = "application/json";

        private readonly IRepository<Election> _electionRepository;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IConsensusNodeService _consensusNodeService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="electionRepository">Repository for elections</param>
        /// <param name="registrationRepository">Repository for registrations</param>
        /// <param name="consensusNodeService">Accessor to consensus node service</param>
        /// <param name="mapper">Automapper</param>
        public ElectionsController(IRepository<Election> electionRepository, IRepository<Registration> registrationRepository, IConsensusNodeService consensusNodeService, IMapper mapper)
        {
            _electionRepository = electionRepository;
            _registrationRepository = registrationRepository;
            _consensusNodeService = consensusNodeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new election.
        /// </summary>
        /// <param name="electionDto">Election parameters</param>
        /// <returns>Newly created election</returns>
        [HttpPost]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<ElectionDto>> Post([FromBody] ElectionDto electionDto)
        {
            Election election = _mapper.Map<Election>(electionDto);

            election = await _electionRepository.CreateAsync(election);

            ElectionDto result = _mapper.Map<ElectionDto>(election);

            return Ok(result);
        }

        /// <summary>
        /// Provides the election with the specified id.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <returns>Election with the specified id.</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces(ContentType)]
        public async Task<ActionResult<ElectionDto>> Get(string id)
        {
            Election election = await _electionRepository.GetAsync(id);

            ElectionDto result = _mapper.Map<ElectionDto>(election);

            return Ok(result);
        }

        /// <summary>
        /// Provides a list of all elections.
        /// </summary>
        /// <returns>List of elections</returns>
        [HttpGet]
        [Produces(ContentType)]
        public async Task<ActionResult<IList<ElectionDto>>> Get()
        {
            IList<Election> elections = await _electionRepository.GetAsync();

            IList<ElectionDto> results = _mapper.Map<IList<ElectionDto>>(elections);

            return Ok(results);
        }

        /// <summary>
        /// Updates a specific election.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <param name="electionDto">Election parameters</param>
        /// <returns>Updated election</returns>
        [HttpPut]
        [Route("{id}")]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<Election>> Put(string id, [FromBody] ElectionDto electionDto)
        {
            Election election = _mapper.Map<Election>(electionDto);

            election = await _electionRepository.UpdateAsync(id, election);

            ElectionDto result = _mapper.Map<ElectionDto>(election);

            return Ok(result);
        }

        /// <summary>
        /// Removes an election.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            await _electionRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Combines and stores the public keys of all registered consensus nodes of the specified election.
        /// </summary>
        /// <param name="id">Election identifier</param>
        /// <returns>Election public key</returns>
        [HttpPut]
        [Route("{id}/public-key")]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<ElectionDto>> Put(string id)
        {
            Election election = await _electionRepository.GetAsync(id);

            election.CombinePublicKeys();

            election = await _electionRepository.UpdateAsync(id, election);

            ElectionDto result = _mapper.Map<ElectionDto>(election);

            return Ok(result);
        }


        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/encrypt")]
        public async Task<ActionResult<Cipher>> Encrypt(string id, Message message)
        {
            Election election = await _electionRepository.GetAsync(id);

            ElGamalCipher cipher = election.Encrypt(message.M);

            return new Cipher
            {
                C = cipher.C.ToString(16),
                D = cipher.D.ToString(16)
            };
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cipher"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/decrypt")]
        public async Task<ActionResult<Message>> Decrypt(string id, Cipher cipher)
        {
            Election election = await _electionRepository.GetAsync(id);

            IList<Registration> consensusNodes = (await _registrationRepository.GetAsync()).Where(r => r.ElectionId == id).ToList();
            IList<string> shares = new List<string>();

            foreach (Registration node in consensusNodes)
            {
                DecryptionShareDto? share = await _consensusNodeService.DecryptShareAsync(node.Endpoint, cipher.C, cipher.D);

                ProofOfDecryption proof = new ProofOfDecryption(new BigInteger(share.ProofOfDecryption.D, 16), 
                    new BigInteger(share.ProofOfDecryption.U, 16),
                    new BigInteger(share.ProofOfDecryption.V, 16),
                    new BigInteger(share.ProofOfDecryption.S, 16));

                bool isValid = proof.Verify(new BigInteger(cipher.C, 16), new BigInteger(cipher.D, 16),
                    new DHPublicKeyParameters(node.PublicKey, election.DhParameters));

                if (!isValid)
                {
                    throw new Exception("Decryption proof is invalid");
                }

                shares.Add(share.DecryptedShare);
            }

            int message = election.CombineShares(shares, cipher.D);

            return new Message
            {
                M = message
            };
        }
    }
}
