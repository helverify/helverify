namespace Helverify.VotingAuthority.Domain.Model.Paper;

/// <summary>
/// Represents an option / candidate in the paper ballot.
/// </summary>
public sealed class PaperBallotOption
{
    /// <summary>
    /// Name of the option / candidate.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Short code of the first virtual ballot
    /// </summary>
    public string ShortCode1 { get; }

    /// <summary>
    /// Short code of the second virtual ballot.
    /// </summary>
    public string ShortCode2 { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">Name of the option / candidate</param>
    /// <param name="shortCode1">Short code of the first virtual ballot</param>
    /// <param name="shortCode2">Short code of the second virtual ballot</param>
    public PaperBallotOption(string name, string shortCode1, string shortCode2)
    {
        Name = name;
        ShortCode1 = shortCode1;
        ShortCode2 = shortCode2;
    }
}