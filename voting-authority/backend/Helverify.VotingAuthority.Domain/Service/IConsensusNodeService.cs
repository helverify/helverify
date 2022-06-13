using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;

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
}