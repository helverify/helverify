using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Model.Consensus;

namespace Helverify.VotingAuthority.Domain.Service;

/// <summary>
/// Service for setting up the Blockchain.
/// </summary>
public interface IBlockchainSetup
{
    /// <summary>
    /// Starts the sealing (~mining) process on each consensus node.
    /// </summary>
    /// <param name="registrations">Consensus node registrations</param>
    /// <returns></returns>
    Task StartSealingAsync(IList<Registration> registrations);

    /// <summary>
    /// Propagates a list of all consensus nodes to each consensus node.
    /// </summary>
    /// <param name="registrations">Consensus node registrations</param>
    /// <param name="nodes">List of Enode identifiers of all consensus nodes in the network.</param>
    /// <returns></returns>
    Task InitializeNodesAsync(IList<Registration> registrations, NodesDto nodes);

    /// <summary>
    /// Connects the consensus nodes to the blockchain.
    /// </summary>
    /// <param name="registrations">Consensus node registrations</param>
    /// <returns></returns>
    Task<NodesDto> StartPeersAsync(IList<Registration> registrations);

    /// <summary>
    /// Propagates the genesis block to all consensus nodes.
    /// </summary>
    /// <param name="registrations">Consensus node registrations</param>
    /// <param name="rpcAccount">Account of this node</param>
    /// <returns></returns>
    Task<Genesis> PropagateGenesisBlockAsync(IList<Registration> registrations, Account rpcAccount);

    /// <summary>
    /// Creates a new blockchain account on every consensus node and updates the address of the account in the registration.
    /// </summary>
    /// <param name="registrations">Consensus node registration</param>
    /// <returns></returns>
    Task<string> CreateAccountsAsync(IList<Registration> registrations);

    /// <summary>
    /// Sets up an RPC endpoint to the blockchain.
    /// </summary>
    /// <param name="genesis">Genesis block information</param>
    /// <param name="nodes">Consensus nodes</param>
    void RegisterRpcEndpoint(Genesis genesis, NodesDto nodes);
}