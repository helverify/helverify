using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Model.Virtual;

namespace Helverify.VotingAuthority.Domain.Service;

/// <summary>
/// Provies access to the consensus node API
/// </summary>
public interface IConsensusNodeService
{
    /// <summary>
    /// Generates a new key pair on the consensus node
    /// </summary>
    /// <param name="endpoint">Consensus node's REST endpoint address</param>
    /// <param name="election">Election (parameters)</param>
    /// <returns></returns>
    Task<PublicKeyDto?> GenerateKeyPairAsync(Uri? endpoint, Election election);

    /// <summary>
    /// Uses the consensus node to decrypt the share of a ciphertext.
    /// </summary>
    /// <param name="endpoint">Consensus node's REST endpoint address</param>
    /// <param name="c">First component of ElGamal cipher (c)</param>
    /// <param name="d">Second component of ElGamal cipher (d)</param>
    /// <returns></returns>
    Task<DecryptionShareDto?> DecryptShareAsync(Uri endpoint, string c, string d);

    /// <summary>
    /// Initializes the genesis block on a consensus node.
    /// </summary>
    /// <param name="endpoint">Consensus node's REST endpoint address</param>
    /// <param name="genesis">Genesis block specification</param>
    /// <returns></returns>
    Task InitializeGenesisBlockAsync(Uri endpoint, Genesis genesis);

    /// <summary>
    /// Creates a new Blockchain account on the consensus node.
    /// </summary>
    /// <param name="endpoint">Consensus node's REST endpoint address</param>
    /// <returns>Account address of the consensus node</returns>
    Task<string> CreateBcAccountAsync(Uri endpoint);

    /// <summary>
    /// Starts geth on the consensus node.
    /// </summary>
    /// <param name="endpoint">Consensus node's REST endpoint address</param>
    /// <returns>Enode id of the consensus node</returns>
    Task<string> StartPeersAsync(Uri endpoint);

    /// <summary>
    /// Announces the other peers contained in nodesDto to the consensus node.
    /// </summary>
    /// <param name="endpoint">Consensus node's REST endpoint address</param>
    /// <param name="nodesDto">List of peers (enode ids)</param>
    /// <returns></returns>
    Task InitializeNodesAsync(Uri endpoint, NodesDto nodesDto);

    /// <summary>
    /// Starts the sealing (PoA "mining") process on the consensus node.
    /// </summary>
    /// <param name="endpoint">Consensus node's REST endpoint address</param>
    /// <returns></returns>
    Task StartSealingAsync(Uri endpoint);

    /// <summary>
    /// Starts the decryption process returning a decrypted share of the ballot.
    /// </summary>
    /// <param name="endpoint">Consensus node's REST endpoint address</param>
    /// <param name="ballot">Ballot to be decrypted</param>
    /// <param name="electionId">Identifier of current election</param>
    /// <param name="ipfsCid">Storage address of the encrypted ballot on IPFS</param>
    /// <returns></returns>
    Task<DecryptedBallotShareDto?> DecryptBallotAsync(Uri endpoint, VirtualBallot ballot, string electionId, string ipfsCid);
}