namespace Helverify.VotingAuthority.Backend.Dto;

public class BallotQrData
{
    public string ElectionId { get; set; }
    public string BallotId { get; set; }

    public BallotQrData()
    {
        
    }

    public BallotQrData(string electionId, string ballotId)
    {
        ElectionId = electionId;
        BallotId = ballotId;
    }
}