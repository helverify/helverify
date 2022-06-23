namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public struct PaperBallotDao
    {
        public PaperBallotDao()
        {
            BallotId = null;
        }

        public string BallotId { get; set; }
        public IList<VirtualBallotDao> Ballots { get; set; } = new List<VirtualBallotDao>(2);
    }
}
