namespace Helverify.VotingAuthority.Domain.Model;

public struct ElectionResults
{
    public IList<ElectionResult> Results { get; }

    public ElectionResults(IList<ElectionResult> results)
    {
        Results = results;
    }
}