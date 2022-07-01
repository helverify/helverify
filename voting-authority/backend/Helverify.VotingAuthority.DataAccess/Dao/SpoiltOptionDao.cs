namespace Helverify.VotingAuthority.DataAccess.Dao;

public struct SpoiltOptionDao
{
    public string Name { get; set; }

    public string ShortCode { get; set; }

    public IList<int> Values { get; set; }

    public SpoiltOptionDao()
    {
        Name = string.Empty;
        ShortCode = string.Empty;
        Values = new List<int>();
    }
}