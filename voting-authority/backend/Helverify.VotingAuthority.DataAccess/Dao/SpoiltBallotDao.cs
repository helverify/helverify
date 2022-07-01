namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public struct SpoiltBallotDao
    {
        public IList<SpoiltOptionDao> Options { get; set; }

        public SpoiltBallotDao()
        {
            Options = new List<SpoiltOptionDao>();
        }
    }
}
