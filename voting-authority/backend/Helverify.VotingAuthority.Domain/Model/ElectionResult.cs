namespace Helverify.VotingAuthority.Domain.Model;

public struct ElectionResult
{
    public ElectionResult()
    {
        OptionName = string.Empty;
        Count = 0;
    }

    public string OptionName { get; set; }
    public int Count { get; set; }
}