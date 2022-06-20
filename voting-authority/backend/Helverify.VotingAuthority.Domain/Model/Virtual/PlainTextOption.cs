namespace Helverify.VotingAuthority.Domain.Model.Virtual;

/// <summary>
/// Represents an election option in plain text.
/// </summary>
public class PlainTextOption
{
    /// <summary>
    /// Short code of this option. Consists of the hash value of all encryptions.
    /// </summary>
    public string ShortCode { get; set; }

    /// <summary>
    /// Option name (i.e., candidate)
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Vector representing this option / candidate. Contains the value 1 for the selected candidate, 0 for all others.
    /// </summary>
    public IList<int> Values { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">Option / candidate name</param>
    /// <param name="values">Vector representing this option</param>
    public PlainTextOption(string name, IList<int> values)
    {
        Name = name;
        Values = values;
        ShortCode = string.Empty;
    }

    /// <summary>
    /// Creates a copy of this option.
    /// </summary>
    /// <returns></returns>
    public PlainTextOption Clone()
    {
        return new PlainTextOption(Name, Values);
    }
}