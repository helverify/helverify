using AutoMapper;
using Helverify.ConsensusNode.Backend.Dto;
using Helverify.ConsensusNode.Domain.Model;
using Helverify.Cryptography.ZeroKnowledge;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;

namespace Helverify.ConsensusNode.Backend.Controllers
{
    /// <summary>
    /// Controller for handling key pairs.
    /// </summary>
    [Route("api/key-pair")]
    [ApiController]
    public class KeyPairController: ControllerBase
    {
        private readonly IKeyPairHandler _keyPairHandler;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keyPairHandler">Key pair service</param>
        /// <param name="mapper">Automapper</param>
        public KeyPairController(IKeyPairHandler keyPairHandler, IMapper mapper)
        {
            _keyPairHandler = keyPairHandler;
            _mapper = mapper;
        }

        /// <summary>
        /// Generates a new key pair and stores it to the file system.
        /// </summary>
        /// <param name="requestDto">Public parameters of the ElGamal cryptosystem</param>
        /// <returns>The public key of this consensus node</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<PublicKeyDto> Post(KeyPairRequestDto requestDto)
        {
            BigInteger p = new BigInteger(requestDto.P, 16);
            BigInteger g = new BigInteger(requestDto.G, 16);
            string electionId = requestDto.ElectionId;

            AsymmetricCipherKeyPair keyPair = _keyPairHandler.CreateKeyPair(p, g);

            _keyPairHandler.SaveToDisk(keyPair, electionId);
            
            return Get(electionId);
        }

        /// <summary>
        /// Returns the current public key of this consensus node.
        /// </summary>
        /// <param name="electionId">Election identifier</param>
        /// <returns>The public key of this consensus node</returns>
        [HttpGet]
        [Route("public-key")]
        [Produces("application/json")]
        public ActionResult<PublicKeyDto> Get([FromQuery] string electionId)
        {
            AsymmetricCipherKeyPair keyPair = _keyPairHandler.LoadFromDisk(electionId);

            ProofOfPrivateKeyOwnership proof = _keyPairHandler.GeneratePrivateKeyProof(keyPair);

            PublicKeyDto publicKeyDto = _mapper.Map<PublicKeyDto>((keyPair.Public, proof));

            return publicKeyDto;
        }
    }
}
