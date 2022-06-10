using AutoMapper;
using Helverify.ConsensusNode.Backend.Dto;
using Helverify.ConsensusNode.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace Helverify.ConsensusNode.Backend.Controllers
{
    [Route("api/decryption")]
    [ApiController]
    public class DecryptionController: ControllerBase
    {
        private readonly IKeyPairHandler _keyPairHandler;
        private readonly IMapper _mapper;

        public DecryptionController(IKeyPairHandler keyPairHandler, IMapper mapper)
        {
            _keyPairHandler = keyPairHandler;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<DecryptionShareDto> Post(EncryptedShareRequestDto requestDto)
        {
            AsymmetricCipherKeyPair keyPair = _keyPairHandler.LoadFromDisk();
            
            EncryptedShare encryptedShare = _mapper.Map<EncryptedShare>(requestDto);

            DecryptedShare decryptedShare = encryptedShare.Decrypt(keyPair);

            DecryptionShareDto response = _mapper.Map<DecryptionShareDto>(decryptedShare);

            return response;
        }
    }
}
