using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.DataAccess.Rest;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;

namespace Helverify.VotingAuthority.Domain.Service
{
    /// <inheritdoc cref="IConsensusNodeService"/>
    internal class ConsensusNodeService : IConsensusNodeService
    {
        private const string KeyPairRoute = "/api/key-pair";
        private const string DecryptionRoute = "/api/decryption";
        private const string BcGenesisRoute = "/api/blockchain/genesis";
        private const string BcAddressRoute = "/api/blockchain/account";
        private const string BcPeerRoute = "/api/blockchain/peer";
        private const string BcNodesRoute = "/api/blockchain/nodes";
        private const string BcSealingRoute = "/api/blockchain/sealing";

        private readonly IRestClient _restClient;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="restClient">Rest client for executing HTTP calls</param>
        public ConsensusNodeService(IRestClient restClient, IMapper _mapper)
        {
            _restClient = restClient;
            this._mapper = _mapper;
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

        public async Task InitializeGenesisBlock(Uri endpoint, Genesis genesis)
        {
            GenesisDto genesisDto = _mapper.Map<GenesisDto>(genesis);

            await _restClient.Call(HttpMethod.Post, new Uri(endpoint, BcGenesisRoute), genesisDto);
        }

        public async Task<string> CreateBcAccount(Uri endpoint)
        {
            return await _restClient.Call<string>(HttpMethod.Post, new Uri(endpoint, BcAddressRoute)) ?? string.Empty;
        }

        public async Task<string> StartPeers(Uri endpoint)
        {
            return await _restClient.Call<string>(HttpMethod.Post, new Uri(endpoint, BcPeerRoute)) ?? string.Empty;
        }

        public async Task InitializeNodes(Uri endpoint, NodesDto nodesDto)
        {
            await _restClient.Call(HttpMethod.Post, new Uri(endpoint, BcNodesRoute), nodesDto);
        }

        public async Task StartSealing(Uri endpoint)
        {
            await _restClient.Call(HttpMethod.Post, new Uri(endpoint, BcSealingRoute));
        }
    }
}
