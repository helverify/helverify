using Helverify.Cryptography.Encryption.Strategy;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption
{
    /// <summary>
    /// Exponential (additive) ElGamal implementation.
    /// </summary>
    public class ExponentialElGamal: ElGamalBase
    {
        private readonly IDecryptionStrategy _decryptionStrategy;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExponentialElGamal()
        {
            _decryptionStrategy = new LinearDecryption();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="decryptionStrategy">Strategy for how to decrypt the ciphertext</param>
        public ExponentialElGamal(IDecryptionStrategy decryptionStrategy)
        {
            _decryptionStrategy = decryptionStrategy;
        }

        /// <inheritdoc cref="ElGamalBase.PrepareMessage"/>
        protected override BigInteger PrepareMessage(int message, BigInteger p, BigInteger generator)
        {

            return generator.ModPow(new BigInteger(message.ToString(), 10), p).Mod(p);
        }

        /// <inheritdoc cref="ElGamalBase.RestoreMessage"/>
        protected override int RestoreMessage(BigInteger m, BigInteger p, BigInteger generator)
            => _decryptionStrategy.Decrypt(generator, p, m);
    }
}
