namespace Helverify.VotingAuthority.Domain.Model.Paper;

public class PaperBallotOption
{
    public string Name { get; set; }
    public string ShortCode1 { get; set; }
    public string ShortCode2 { get; set; }

    public PaperBallotOption(string name, string shortCode1, string shortCode2)
    {
        Name = name;
        ShortCode1 = shortCode1;
        ShortCode2 = shortCode2;
    }
}