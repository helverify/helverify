using Helverify.VotingAuthority.Domain.Helper;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Domain.Model;

public class EncryptedOption
{
    public IList<EncryptedOptionValue> Values { get; }
    public string ShortCode { get; internal set; }
    public string Hash { get;  }

    public EncryptedOption(DHPublicKeyParameters publicKey, IList<int> plainTextOption)
    {
        Values = new List<EncryptedOptionValue>();

        foreach (int option in plainTextOption)
        {
            EncryptedOptionValue encryptedOptionValue = new EncryptedOptionValue(option, publicKey);

            Values.Add(encryptedOptionValue);
        }

        Hash = HashHelper.Hash(Values.Select(v => v.Cipher).ToArray());
        ShortCode = Hash.Substring(0, 2);
    }
}