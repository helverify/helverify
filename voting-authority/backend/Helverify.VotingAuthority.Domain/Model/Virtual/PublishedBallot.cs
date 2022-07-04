namespace Helverify.VotingAuthority.Domain.Model.Virtual
{
    /// <summary>
    /// Represents a ballot published on the smart contract
    /// </summary>
    public struct PublishedBallot
    {
        public string BallotId { get; set; }
        public string BallotCode { get; set; }
        public string IpfsCid { get; set; }
    }
}
