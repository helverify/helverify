using Helverify.VotingAuthority.Domain.Helper;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Domain.Model.Virtual;

/// <summary>
/// Represents an encrypted option.
/// </summary>
public sealed class EncryptedOption
{
    /// <summary>
    /// List of encryptions and corresponding proofs
    /// </summary>
    public IList<EncryptedOptionValue> Values { get; }

    /// <summary>
    /// Short code of this option / candidate
    /// </summary>
    public string ShortCode { get; internal init; }

    /// <summary>
    /// Complete hash of all encryptions
    /// </summary>
    public string Hash { get; }

    public EncryptedOption()
    {
        Values = new List<EncryptedOptionValue>();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="publicKey">Public key of the election</param>
    /// <param name="plainTextOption">Option in plaintext, list of zeros and a one (position of one = selected candidate / option)</param>
    public EncryptedOption(DHPublicKeyParameters publicKey, IList<int> plainTextOption)
    {
        Values = new List<EncryptedOptionValue>();

        foreach (int option in plainTextOption)
        {
            EncryptedOptionValue encryptedOptionValue = new EncryptedOptionValue(option, publicKey);

            Values.Add(encryptedOptionValue);
        }

        HashHelper hashHelper = new HashHelper();

        Hash = hashHelper.Hash(Values.Select(v => v.Cipher).ToArray());

        ShortCode = Hash.Substring(0, 2);
    }
}