using AutoMapper;
using Helverify.ConsensusNode.Backend.Dto;
using Helverify.ConsensusNode.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace Helverify.ConsensusNode.Backend.Controllers
{
    /// <summary>
    /// Controller for cooperative decryption
    /// </summary>
    [Route("api/decryption")]
    [ApiController]
    public class DecryptionController: ControllerBase
    {
        private readonly IKeyPairHandler _keyPairHandler;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keyPairHandler">Key pair service</param>
        /// <param name="mapper">Automapper</param>
        public DecryptionController(IKeyPairHandler keyPairHandler, IMapper mapper)
        {
            _keyPairHandler = keyPairHandler;
            _mapper = mapper;
        }

        /// <summary>
        /// Decrypts this node's share of the specified ciphertext.
        /// </summary>
        /// <param name="requestDto">CipherText of an ElGamal cryptosystem</param>
        /// <returns>Decrypted share</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<DecryptionShareDto> Post(EncryptedShareRequestDto requestDto)
        {
            AsymmetricCipherKeyPair keyPair = _keyPairHandler.LoadFromDisk(requestDto.ElectionId);
            
            CipherText cipherText = _mapper.Map<CipherText>(requestDto);

            DecryptedShare decryptedShare = cipherText.Decrypt(keyPair);

            DecryptionShareDto response = _mapper.Map<DecryptionShareDto>(decryptedShare);

            return response;
        }
    }
}
