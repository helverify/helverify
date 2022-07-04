namespace Helverify.VotingAuthority.Backend.Dto;

/// <summary>
/// Represents one selectable option / candidate
/// </summary>
public class PrintOptionDto
{
    /// <summary>
    /// Name of the option / candidate
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// First short code (from first virtual ballot)
    /// </summary>
    public string ShortCode1 { get; set; }

    /// <summary>
    /// Second short code (from second virtual ballot)
    /// </summary>
    public string ShortCode2 { get; set; }
}