using AutoMapper;
using Helverify.ConsensusNode.DataAccess.Dao;
using Helverify.ConsensusNode.DataAccess.Ipfs;
using Helverify.ConsensusNode.Domain.Model;

namespace Helverify.ConsensusNode.Domain.Repository
{
    public class BallotRepository : IBallotRepository
    {
        private readonly IStorageClient _storageClient;
        private readonly IMapper _mapper;

        public BallotRepository(IStorageClient storageClient, IMapper mapper)
        {
            _storageClient = storageClient;
            _mapper = mapper;
        }

        public async Task<BallotEncryption> GetBallotEncryption(string cid)
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
