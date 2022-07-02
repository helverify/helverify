namespace Helverify.VotingAuthority.Domain.Model.Virtual
{
    public struct PublishedBallot
    {
        public string BallotId { get; set; }
        public string BallotCode { get; set; }
        public string IpfsCid { get; set; }
    }
}
