using Helverify.Cryptography.Encryption.Strategy;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption
{
    public class ExponentialElGamal: ElGamalBase
    {
        private readonly IDecryptionStrategy _decryptionStrategy;

        public ExponentialElGamal()
        {
            _decryptionStrategy = new LinearDecryption();
        }

        public ExponentialElGamal(IDecryptionStrategy decryptionStrategy)
        {
            _decryptionStrategy = decryptionStrategy;
        }

        protected override BigInteger PrepareM(BigInteger generator, BigInteger p, int message)
        {

            return generator.ModPow(new BigInteger(message.ToString(), 10), p).Mod(p);
        }

        protected override int RestoreMessage(BigInteger generator, BigInteger p, BigInteger m)
            => _decryptionStrategy.Decrypt(generator, p, m);
    }
}
