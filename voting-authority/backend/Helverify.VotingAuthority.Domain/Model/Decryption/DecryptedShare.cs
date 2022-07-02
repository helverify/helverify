using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Model.Decryption
{
    /// <summary>
    /// Represents a single decrypted share of a ciphertext.
    /// </summary>
    public struct DecryptedShare
    {
        /// <summary>
        /// Decrypted share value
        /// </summary>
        public BigInteger Share { get; set; }

        /// <summary>
        /// Proof of correct decryption
        /// </summary>
        public ProofOfDecryption ProofOfDecryption { get; set; }

        /// <summary>
        /// Public key share of the decrypting consensus node
        /// </summary>
        public BigInteger PublicKeyShare { get; set; }
    }

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

    /// <summary>
    /// Contains all shares of a decrypted ballot.
    /// </summary>
    public class BallotShares
    {
        /// <summary>
        /// Contains the shares of all options of this ballot
        /// </summary>
        public IList<OptionShare> OptionShares { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="optionShares">Decrypted shares of options</param>
        public BallotShares(IList<OptionShare> optionShares)
        {
            OptionShares = optionShares;
        }

        /// <summary>
        /// Reconstructs the plaintext ballot form the decrypted shares.
        /// </summary>
        /// <param name="election">Election</param>
        /// <param name="virtualBallot">Ballot to be merged into</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
}
