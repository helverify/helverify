using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Service;

namespace Helverify.VotingAuthority.Domain.Model.Blockchain
{
    /// <summary>
    /// Facade for setting up an Ethereum PoA blockchain across multiple consensus nodes.
    /// </summary>
    public class BlockchainSetup
    {
        private const string InitialFunds = "1000000000000000000000000000000000000";

        private readonly IConsensusNodeService _consensusNodeService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consensusNodeService">Service facade for access to consensus node operations</param>
        public BlockchainSetup(IConsensusNodeService consensusNodeService)
        {
            _consensusNodeService = consensusNodeService;
        }

        /// <summary>
        /// Starts the sealing (~mining) process on each consensus node.
        /// </summary>
        /// <param name="registrations">Consensus node registrations</param>
        /// <returns></returns>
        public async Task StartSealingAsync(IList<Registration> registrations)
        {
            foreach (Registration registration in registrations)
            {
                await _consensusNodeService.StartSealing(registration.Endpoint);
            }
        }

        /// <summary>
        /// Propagates a list of all consensus nodes to each consensus node.
        /// </summary>
        /// <param name="registrations">Consensus node registrations</param>
        /// <param name="nodes">List of Enode identifiers of all consensus nodes in the network.</param>
        /// <returns></returns>
        public async Task InitializeNodesAsync(IList<Registration> registrations, NodesDto nodes)
        {
            foreach (Registration registration in registrations)
            {
                await _consensusNodeService.InitializeNodes(registration.Endpoint, nodes);
            }
        }

        /// <summary>
        /// Connects the consensus nodes to the blockchain.
        /// </summary>
        /// <param name="registrations">Consensus node registrations</param>
        /// <returns></returns>
        public async Task<NodesDto> StartPeersAsync(IList<Registration> registrations)
        {
            foreach (Registration registration in registrations)
            {
                registration.Enode = await _consensusNodeService.StartPeers(registration.Endpoint);
            }

            NodesDto nodes = new NodesDto
            {
                Nodes = registrations.Select(r => r.Enode).ToList()
            };

            return nodes;
        }

        /// <summary>
        /// Propagates the genesis block to all consensus nodes.
        /// </summary>
        /// <param name="registrations">Consensus node registrations</param>
        /// <returns></returns>
        public async Task PropagateGenesisBlockAsync(IList<Registration> registrations)
        {
            IList<Account> authorities = registrations.Select(r => r.Account).ToList();

            Genesis genesis = new Genesis(13337, authorities, authorities);

            foreach (Registration registration in registrations)
            {
                await _consensusNodeService.InitializeGenesisBlock(registration.Endpoint, genesis);
            }
        }

        /// <summary>
        /// Creates a new blockchain account on every consensus node and updates the address of the account in the registration.
        /// </summary>
        /// <param name="registrations">Consensus node registration</param>
        /// <returns></returns>
        public async Task CreateAccountsAsync(IList<Registration> registrations)
        {
            foreach (Registration registration in registrations)
            {
                string bcAddress = await _consensusNodeService.CreateBcAccount(registration.Endpoint);

                registration.Account = new Account(bcAddress, InitialFunds);
            }
        }
    }
}
