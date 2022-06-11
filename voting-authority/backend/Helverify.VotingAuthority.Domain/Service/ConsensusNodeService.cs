using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.DataAccess.Rest;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Service
{
    internal class ConsensusNodeService : IConsensusNodeService
    {
        private const string KeyPairRoute = "/api/key-pair";
        private const string DecryptionRoute = "/api/decryption";

        private readonly IRestClient _restClient;

        public ConsensusNodeService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<PublicKeyDto> GenerateKeyPairAsync(Uri endpoint, Election election)
        {
            return await _restClient.Call<PublicKeyDto>(HttpMethod.Post, new Uri(endpoint, KeyPairRoute),
                new KeyPairRequestDto { P = election.P, G = election.G });
        }

        public async Task<DecryptionShareDto> DecryptShareAsync(Uri endpoint, string c, string d)
        {
            return await _restClient.Call<DecryptionShareDto>(HttpMethod.Post, new Uri(endpoint, DecryptionRoute),
            new EncryptedShareRequestDto
            {
                Cipher = new CipherTextDto
                {
                    C = c,
                    D = d,
                },
            });
        }
    }
}
