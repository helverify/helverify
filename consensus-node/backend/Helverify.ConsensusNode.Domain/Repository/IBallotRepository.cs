using Helverify.ConsensusNode.Domain.Model;

namespace Helverify.ConsensusNode.Domain.Repository;

/// <summary>
/// Provides access to encrypted ballots
/// </summary>
public interface IBallotRepository
{
    /// <summary>
    /// Retrieves encrypted ballots from IPFS
    /// </summary>
    /// <param name="cid">IPFS cid (address) of the encrypted ballot</param>
    /// <returns></returns>
    Task<BallotEncryption> GetBallotEncryptionAsync(string cid);
}