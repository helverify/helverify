namespace Helverify.VotingAuthority.DataAccess.Dao;

public class PrintOptionDao
{
    public string Name { get; set; }
    public string ShortCode1 { get; set; }
    public string ShortCode2 { get; set; }
    public IList<string> Randomness1 { get; set; } = new List<string>();
    public IList<string> Randomness2 { get; set; } = new List<string>();
}