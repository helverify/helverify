namespace Helverify.VotingAuthority.DataAccess.Dao;

public struct SpoiltOptionDao
{
    public string Name { get; set; }

    public string ShortCode { get; set; }

    public IList<int> Values { get; set; }

    public IList<string> Randomness { get; set; }

    public SpoiltOptionDao()
    {
        Name = string.Empty;
        ShortCode = string.Empty;
        Values = new List<int>();
        Randomness = new List<string>();
    }

    public SpoiltOptionDao SetRandomness(IList<string> randomValues)
    {
        Randomness = randomValues;

        return this;
    }
}