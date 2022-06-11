using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    [Route("api/elections")]
    [ApiController]
    public class ElectionsController : ControllerBase
    {
        private const string ContentType = "application/json";

        private readonly IMongoService<Election> _electionService;
        private readonly IMongoService<Registration> _registrationService;
        private readonly IConsensusNodeService _consensusNodeService;

        public ElectionsController(IMongoService<Election> electionService, IMongoService<Registration> registrationService, IConsensusNodeService consensusNodeService)
        {
            _electionService = electionService;
            _registrationService = registrationService;
            _consensusNodeService = consensusNodeService;
        }

        [HttpPost]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<Election>> Post([FromBody] Election election)
        {
            await _electionService.CreateAsync(election);

            return Ok(election);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces(ContentType)]
        public async Task<ActionResult<Election>> Get(string id)
        {
            return await _electionService.GetAsync(id);
        }

        [HttpGet]
        [Produces(ContentType)]
        public async Task<ActionResult<List<Election>>> Get()
        {
            return await _electionService.GetAsync();
        }

        [HttpPut]
        [Route("{id}")]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<Election>> Put(string id, [FromBody] Election election)
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

        [HttpPut]
        [Route("{id}/public-key")]
        [Consumes(ContentType)]
        [Produces(ContentType)]
        public async Task<ActionResult<Election>> Put(string id)
        {
            Election election = await _electionService.GetAsync(id);

            IEnumerable<Registration> registrations = (await _registrationService.GetAsync()).Where(r => r.ElectionId == id);

            election.CombinePublicKeys(registrations);

            await _electionService.UpdateAsync(id, election);

            return election;
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
            Election election = await _electionService.GetAsync(id);

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
            Election election = await _electionService.GetAsync(id);

            IList<Registration> consensusNodes = (await _registrationService.GetAsync()).Where(r => r.ElectionId == id).ToList();
            IList<string> shares = new List<string>();

            foreach (Registration node in consensusNodes)
            {
                DecryptionShareDto share = await _consensusNodeService.DecryptShareAsync(node.Endpoint, cipher.C, cipher.D);

                ProofOfDecryption proof = new ProofOfDecryption(new BigInteger(share.ProofOfDecryption.D, 16), 
                    new BigInteger(share.ProofOfDecryption.U, 16),
                    new BigInteger(share.ProofOfDecryption.V, 16),
                    new BigInteger(share.ProofOfDecryption.S, 16));

                bool isValid = proof.Verify(new BigInteger(cipher.C, 16), new BigInteger(cipher.D, 16),
                    new DHPublicKeyParameters(new BigInteger(node.PublicKey, 16), election.DhParameters));

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
