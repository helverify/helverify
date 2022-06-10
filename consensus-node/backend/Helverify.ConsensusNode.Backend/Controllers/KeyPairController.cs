using System.Diagnostics;
using AutoMapper;
using Helverify.ConsensusNode.Backend.Dto;
using Helverify.ConsensusNode.Domain.Model;
using Helverify.Cryptography.ZeroKnowledge;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;

namespace Helverify.ConsensusNode.Backend.Controllers
{
    [Route("api/key-pair")]
    [ApiController]
    public class KeyPairController: ControllerBase
    {
        private readonly IKeyPairHandler _keyPairHandler;
        private readonly IMapper _mapper;

        public KeyPairController(IKeyPairHandler keyPairHandler, IMapper mapper)
        {
            _keyPairHandler = keyPairHandler;
            _mapper = mapper;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<PublicKeyDto> Post(KeyPairRequestDto requestDto)
        {
            BigInteger p = new BigInteger(requestDto.P, 16);
            BigInteger g = new BigInteger(requestDto.G, 16);

            AsymmetricCipherKeyPair keyPair = _keyPairHandler.CreateKeyPair(p, g);

            _keyPairHandler.SaveToDisk(keyPair);
            
            return Get();
        }

        [HttpGet]
        [Route("public-key")]
        [Produces("application/json")]
        public ActionResult<PublicKeyDto> Get()
        {
            AsymmetricCipherKeyPair keyPair = _keyPairHandler.LoadFromDisk();

            ProofOfPrivateKeyOwnership proof = _keyPairHandler.GeneratePrivateKeyProof(keyPair);

            PublicKeyDto publicKeyDto = _mapper.Map<PublicKeyDto>((keyPair.Public, proof));

            return publicKeyDto;
        }
    }
}
