namespace Helverify.VotingAuthority.DataAccess.Dto
{
    public class EncryptedBallotDto
    {
        public string ElectionId { get; set; }
        public string BallotCode { get; set;  }
        public string IpfsCid { get; set; }

        public EncryptedBallotDto(string electionId, string ballotCode, string ipfsCid)
        {
            ElectionId = electionId;
            BallotCode = ballotCode;
            IpfsCid = ipfsCid;
        }
    }
}
