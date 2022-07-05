using Helverify.Cryptography.Encryption;

namespace Helverify.VotingAuthority.Domain.Model.Decryption
{
    /// <summary>
    /// Represents a plaintext message
    /// </summary>
    public class DecryptedValue
    {
        /// <summary>
        /// Numeric message
        /// </summary>
        public int PlainText { get; set; }

        public ElGamalCipher CipherText { get; set; }

        public IList<DecryptedShare> Shares { get; set; } = new List<DecryptedShare>();
    }
}
