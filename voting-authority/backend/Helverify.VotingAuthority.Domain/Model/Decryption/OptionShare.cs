namespace Helverify.VotingAuthority.Domain.Model.Decryption;

/// <summary>
/// Represents a decrypted share of an entire option (e.g., representing plaintext [0, 0, 1])
/// </summary>
public struct OptionShare
{
    /// <summary>
    /// Constructor
    /// </summary>
    public OptionShare()
    {
        ShortCode = null;
        Shares = new List<DecryptedShare>();
    }

    /// <summary>
    /// Short code of the option
    /// </summary>
    public string ShortCode { get; set; }

    /// <summary>
    /// Decrypted shares of each atomic encryption
    /// </summary>
    public IList<DecryptedShare> Shares { get; set; } 
}