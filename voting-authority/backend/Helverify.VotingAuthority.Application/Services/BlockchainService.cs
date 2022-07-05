using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;

namespace Helverify.VotingAuthority.Application.Services
{
    /// <inheritdoc cref="IBlockchainService"/>
    internal class BlockchainService : IBlockchainService
    {
        private readonly IBlockchainSetup _blockchainSetup;
        private readonly IRepository<Blockchain> _bcRepository;

        public BlockchainService(IBlockchainSetup blockchainSetup, IRepository<Blockchain> bcRepository)
        {
            _blockchainSetup = blockchainSetup;
            _bcRepository = bcRepository;
        }

        /// <inheritdoc cref="IBlockchainService.Initialize"/>
        public async Task<Blockchain> Initialize(Blockchain blockchain)
        {
            IList<Registration> registrations = blockchain.Registrations;

            string nodeAddress = await _blockchainSetup.CreateAccountsAsync(registrations);

            Genesis genesis = await _blockchainSetup.PropagateGenesisBlockAsync(registrations, new Account(nodeAddress, "1000000000000000000000000000000000000000000000"));

            NodesDto nodes = await _blockchainSetup.StartPeersAsync(registrations);

            await _blockchainSetup.InitializeNodesAsync(registrations, nodes);

            await _blockchainSetup.StartSealingAsync(registrations);

            _blockchainSetup.RegisterRpcEndpoint(genesis, nodes);

            blockchain = await _bcRepository.CreateAsync(blockchain);

            return blockchain;
        }

        /// <inheritdoc cref="IBlockchainService.GetInstance"/>
        public async Task<Blockchain?> GetInstance() =>
            (await _bcRepository.GetAsync()).MaxBy(bc => bc.Id);
    }
}
