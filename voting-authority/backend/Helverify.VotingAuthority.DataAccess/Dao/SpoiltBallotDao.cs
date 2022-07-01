using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public struct SpoiltBallotDao
    {
        public IList<SpoiltOptionDao> Options { get; set; }

        public SpoiltBallotDao()
        {
            Options = new List<SpoiltOptionDao>();
        }

        public void SetRandomness(IDictionary<string,IList<BigInteger>> randomness)
        {
            for (int i = 0; i < Options.Count; i++)
            {
                string shortCode = Options[i].ShortCode;

                IList<string> randomValues = randomness[shortCode].Select(r => r.ToString(16)).ToList();

                Options[i] = Options[i].SetRandomness(randomValues);
            }
        }
    }
}
