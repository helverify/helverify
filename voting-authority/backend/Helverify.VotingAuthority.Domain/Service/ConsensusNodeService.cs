using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.DataAccess.Rest;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Service
{
    /// <inheritdoc cref="IConsensusNodeService"/>
    internal class ConsensusNodeService : IConsensusNodeService
    {
        private const string KeyPairRoute = "/api/key-pair";
        private const string DecryptionRoute = "/api/decryption";

        private readonly IRestClient _restClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="restClient">Rest client for executing HTTP calls</param>
        public ConsensusNodeService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        /// <inheritdoc cref="IConsensusNodeService.GenerateKeyPairAsync"/>
        public async Task<PublicKeyDto?> GenerateKeyPairAsync(Uri? endpoint, Election election)
        {
            if (endpoint == null)
            {
                throw new ArgumentException("Endpoint must be specified before calling it.", nameof(endpoint));
            }

            return await _restClient.Call<PublicKeyDto>(HttpMethod.Post, new Uri(endpoint, KeyPairRoute),
                new KeyPairRequestDto { P = election.P.ConvertToHexString(), G = election.G.ConvertToHexString() });
        }

        /// <inheritdoc cref="IConsensusNodeService.DecryptShareAsync"/>
        public async Task<DecryptionShareDto?> DecryptShareAsync(Uri endpoint, string c, string d)
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
