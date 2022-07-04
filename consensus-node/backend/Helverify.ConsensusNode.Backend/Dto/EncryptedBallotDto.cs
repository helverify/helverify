namespace Helverify.ConsensusNode.Backend.Dto;

/// <summary>
/// Represents an encrypted ballot to be decrypted
/// </summary>
public class EncryptedBallotDto
{
    /// <summary>
    /// Election identifier
    /// </summary>
    public string ElectionId { get; set; }

    /// <summary>
    /// Ballot identifier
    /// </summary>
    public string BallotCode { get; set; }

    /// <summary>
    /// IPFS cid reference to retrieve the encrypted ballot
    /// </summary>
    public string IpfsCid { get; set; }
}