using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Model.Decryption
{
    public class DecryptedShare
    {
        public BigInteger Share { get; set; }

        public ProofOfDecryption ProofOfDecryption { get; set; }

        public BigInteger PublicKeyShare { get; set; }
    }

    public class OptionShare
    {
        public string ShortCode { get; set; }

        public IList<DecryptedShare> Shares { get; set; } = new List<DecryptedShare>();
    }

    public class BallotShares
    {
        public IList<OptionShare> OptionShares { get; }

        public BallotShares(IList<OptionShare> optionShares)
        {
            OptionShares = optionShares;
        }

        public VirtualBallot CombineShares(Election election, VirtualBallot virtualBallot)
        {
            foreach (EncryptedOption encryptedOption in virtualBallot.EncryptedOptions)
            {
                IList<int> optionsVector = new List<int>();

                for (int i = 0; i < encryptedOption.Values.Count; i++)
                {
                    EncryptedOptionValue encryptedOptionValue = encryptedOption.Values[i];

                    ElGamalCipher cipher = encryptedOptionValue.Cipher;

                    BigInteger d = cipher.D;

                    IList<OptionShare> os = OptionShares.Where(s => s.ShortCode == encryptedOption.ShortCode).ToList();

                    IList<DecryptedShare> decryptedShares = os.Select(o => o.Shares[i]).ToList(); // decrypted shares of one option vector element

                    if (!decryptedShares.All(d => d.ProofOfDecryption.Verify(cipher.C, cipher.D,
                            new DHPublicKeyParameters(d.PublicKeyShare, election.DhParameters))))
                    {
                        throw new Exception("Proof of decryption is invalid");
                    }

                    int plainTextValue = election.CombineShares(decryptedShares, d);

                    optionsVector.Add(plainTextValue);

                }

                PlainTextOption plainTextOption = new PlainTextOption(election.Options[optionsVector.IndexOf(1)].Name, optionsVector)
                {
                    ShortCode = encryptedOption.ShortCode
                };

                virtualBallot.PlainTextOptions.Add(plainTextOption);
            }

            return virtualBallot;
        }
    }


    public class DecryptedBallot
    {
        public string BallotCode { get; set; }
        public IList<PlainTextOption> PlainTextOptions { get; set; } = new List<PlainTextOption>();
    }
}
