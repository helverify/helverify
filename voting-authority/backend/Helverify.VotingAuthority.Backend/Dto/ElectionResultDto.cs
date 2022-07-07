namespace Helverify.VotingAuthority.Backend.Dto;

/// <summary>
/// Represents the result of a single option / candidate
/// </summary>
public struct ElectionResultDto
{
    /// <summary>
    /// Option / candidate name
    /// </summary>
    public string OptionName { get; set; }

    /// <summary>
    /// Tally of option / candidate
    /// </summary>
    public int Count { get; set; }
}