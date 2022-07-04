using AutoMapper;
using Helverify.ConsensusNode.Backend.Dto;
using Helverify.ConsensusNode.Domain.Model;
using Helverify.ConsensusNode.Domain.Repository;
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
        private readonly IBallotRepository _ballotRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keyPairHandler">Key pair service</param>
        /// <param name="mapper">Automapper</param>
        public DecryptionController(IKeyPairHandler keyPairHandler, IBallotRepository ballotRepository, IMapper mapper)
        {
            _keyPairHandler = keyPairHandler;
            _ballotRepository = ballotRepository;
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

        /// <summary>
        /// Decrypts this node's share of the specified ballot.
        /// </summary>
        /// <param name="requestDto">Contains the data necessary to retrieve an encrypted ballot from IPFS</param>
        /// <returns>Decrypted share</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Route("ballot")]
        public async Task<ActionResult<DecryptedBallotShareDto>> PostBallotDecryption(EncryptedBallotDto requestDto)
        {
            AsymmetricCipherKeyPair keyPair = _keyPairHandler.LoadFromDisk(requestDto.ElectionId);

            BallotEncryption ballotEncryption = await _ballotRepository.GetBallotEncryptionAsync(requestDto.IpfsCid);

            IDictionary<string, IList<DecryptionShareDto>> decryptedShares = new Dictionary<string, IList<DecryptionShareDto>>();

            foreach (string shortCode in ballotEncryption.Encryptions.Keys)
            {
                IList<DecryptionShareDto> shares = new List<DecryptionShareDto>();

                foreach (CipherText cipher in ballotEncryption.Encryptions[shortCode])
                {
                    DecryptedShare decryptedShare = cipher.Decrypt(keyPair);

                    DecryptionShareDto shareDto = _mapper.Map<DecryptionShareDto>(decryptedShare);

                    shares.Add(shareDto);
                }

                decryptedShares[shortCode] = shares;
            }
            
            return new DecryptedBallotShareDto
            {
                DecryptedShares = decryptedShares
            };
        }
    }
}
