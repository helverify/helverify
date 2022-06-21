namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public class PaperBallotDao
    {
        public string BallotId { get; set; }
        public IList<VirtualBallotDao> Ballots { get; set; } = new List<VirtualBallotDao>(2);
    }
}
