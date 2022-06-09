using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption.Strategy;

/// <summary>
/// Defines the strategy for decrypting exponential ElGamal ciphertexts.
/// </summary>
public interface IDecryptionStrategy
{
    /// <summary>
    /// Decrypts an exponential ElGamal ciphertext using brute-force.
    /// </summary>
    /// <param name="cipher"></param>
    /// <param name="p">Prime</param>
    /// <param name="g"></param>
    /// <returns></returns>
    int Decrypt(BigInteger cipher, BigInteger p, BigInteger g);
}