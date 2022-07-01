namespace Helverify.ConsensusNode.Backend.Controllers;

public class EncryptedBallotDto
{
    public string ElectionId { get; set; }
    public string BallotCode { get; set; }
    public string IpfsCid { get; set; }
}