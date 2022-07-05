using AutoMapper;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.DataAccess.Rest;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Service
{
    /// <inheritdoc cref="IConsensusNodeService"/>
    internal class ConsensusNodeService : IConsensusNodeService
    {
        private const string KeyPairRoute = "/api/key-pair";
        private const string DecryptionRoute = "/api/decryption";
        private const string DecryptBallotRoute = "/api/decryption/ballot";
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
                new KeyPairRequestDto { ElectionId = election.Id, P = election.P.ConvertToHexString(), G = election.G.ConvertToHexString() });
        }

        /// <inheritdoc cref="IConsensusNodeService.DecryptShareAsync"/>
        public async Task<DecryptedShare> DecryptShareAsync(Uri endpoint, Election election, ElGamalCipher cipher, BigInteger publicKey)
        {
            DecryptionShareDto share = (await _restClient.Call<DecryptionShareDto>(HttpMethod.Post, new Uri(endpoint, DecryptionRoute),
                new EncryptedShareRequestDto
                {
                    Cipher = new CipherTextDto
                    {
                        C = cipher.C.ConvertToHexString(),
                        D = cipher.D.ConvertToHexString(),
                    },
                    ElectionId = election.Id!
                }))!;

            ProofOfDecryption proof = new ProofOfDecryption(share.ProofOfDecryption.D.ConvertToBigInteger(),
                share.ProofOfDecryption.U.ConvertToBigInteger(),
                share.ProofOfDecryption.V.ConvertToBigInteger(),
                share.ProofOfDecryption.S.ConvertToBigInteger());

            DecryptedShare decryptedShare = new DecryptedShare
            {
                ProofOfDecryption = proof,
                PublicKeyShare = publicKey,
                Share = share.DecryptedShare.ConvertToBigInteger()
            };

            return decryptedShare;
        }

        /// <inheritdoc cref="IConsensusNodeService.InitializeGenesisBlockAsync"/>
        public async Task InitializeGenesisBlockAsync(Uri endpoint, Genesis genesis)
        {
            GenesisDto genesisDto = _mapper.Map<GenesisDto>(genesis);

            await _restClient.Call(HttpMethod.Post, new Uri(endpoint, BcGenesisRoute), genesisDto);
        }

        /// <inheritdoc cref="IConsensusNodeService.CreateBcAccountAsync"/>
        public async Task<string> CreateBcAccountAsync(Uri endpoint)
        {
            return await _restClient.Call<string>(HttpMethod.Post, new Uri(endpoint, BcAddressRoute)) ?? string.Empty;
        }

        /// <inheritdoc cref="IConsensusNodeService.StartPeersAsync"/>
        public async Task<string> StartPeersAsync(Uri endpoint)
        {
            return await _restClient.Call<string>(HttpMethod.Post, new Uri(endpoint, BcPeerRoute)) ?? string.Empty;
        }

        /// <inheritdoc cref="IConsensusNodeService.InitializeNodesAsync"/>
        public async Task InitializeNodesAsync(Uri endpoint, NodesDto nodesDto)
        {
            await _restClient.Call(HttpMethod.Post, new Uri(endpoint, BcNodesRoute), nodesDto);
        }

        /// <inheritdoc cref="IConsensusNodeService.StartSealingAsync"/>
        public async Task StartSealingAsync(Uri endpoint)
        {
            await _restClient.Call(HttpMethod.Post, new Uri(endpoint, BcSealingRoute));
        }

        /// <inheritdoc cref="IConsensusNodeService.DecryptBallotAsync"/>
        public async Task<DecryptedBallotShareDto?> DecryptBallotAsync(Uri endpoint, VirtualBallot ballot, string electionId, string ipfsCid)
        {
            EncryptedBallotDto encryptedBallotDto = new EncryptedBallotDto(electionId, ballot.Code, ipfsCid);

            return await _restClient.Call<DecryptedBallotShareDto>(HttpMethod.Post, new Uri(endpoint, DecryptBallotRoute), encryptedBallotDto);
        }
    }
}
