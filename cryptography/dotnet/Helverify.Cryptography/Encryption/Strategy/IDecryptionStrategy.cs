using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption.Strategy;

public interface IDecryptionStrategy
{
    int Decrypt(BigInteger generator, BigInteger p, BigInteger m);
}