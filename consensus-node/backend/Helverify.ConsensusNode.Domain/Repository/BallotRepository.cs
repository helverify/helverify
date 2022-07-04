using AutoMapper;
using Helverify.ConsensusNode.DataAccess.Dao;
using Helverify.ConsensusNode.DataAccess.Ipfs;
using Helverify.ConsensusNode.Domain.Model;

namespace Helverify.ConsensusNode.Domain.Repository
{
    /// <inheritdoc cref="IBallotRepository"/>
    public class BallotRepository : IBallotRepository
    {
        private readonly IStorageClient _storageClient;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storageClient">IPFS storage client</param>
        /// <param name="mapper">Automapper instance</param>
        public BallotRepository(IStorageClient storageClient, IMapper mapper)
        {
            _storageClient = storageClient;
            _mapper = mapper;
        }

        /// <inheritdoc cref="IBallotRepository.GetBallotEncryptionAsync"/>
        public async Task<BallotEncryption> GetBallotEncryptionAsync(string cid)
        {
            VirtualBallotDao ballot = await _storageClient.Retrieve<VirtualBallotDao>(cid);

            IDictionary<string, IList<CipherText>> cipherDictionary = new Dictionary<string, IList<CipherText>>();
            
            foreach (EncryptedOptionDao encryptedOption in ballot.EncryptedOptions)
            {
                IList<CipherText> cipherTexts = _mapper.Map<IList<CipherText>>(encryptedOption.Values.Select(v => v.Cipher).ToList()); // TODO: mapping profile

                cipherDictionary[encryptedOption.ShortCode] = cipherTexts;
            }

            return new BallotEncryption(cipherDictionary);
        }
    }
}
