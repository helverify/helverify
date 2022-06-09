using System.Runtime.CompilerServices;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

[assembly: InternalsVisibleTo("Helverify.Cryptography.Tests")]
namespace Helverify.Cryptography.Encryption
{

    /// <summary>
    /// Inspired by the following sources:
    /// Bernhard2016
    /// https://github.com/meck93/evote-crypto
    /// https://github.com/meck93/evote-crypto/blob/master/src/ff-elGamal/encryption.ts
    /// https://github.com/bcgit/bc-csharp/
    /// https://nvotes.com/multiplicative-vs-additive-homomorphic-elGamal/
    /// </summary>
    public interface IElGamal
    {
        AsymmetricKeyParameter GetKey(string path);

        DHParameters GetParameters(BigInteger p, BigInteger g);

        AsymmetricCipherKeyPair KeyGen(BigInteger p, BigInteger g, SecureRandom random = null);

        ElGamalCipher Encrypt(int message, AsymmetricKeyParameter publicKeyParameter, BigInteger randomness = null);

        int Decrypt(ElGamalCipher cipher, AsymmetricKeyParameter privateKeyParameter);

        DHPublicKeyParameters CombinePublicKeys(IList<DHPublicKeyParameters> publicKeys,
            DHParameters elGamalParameters);

        BigInteger DecryptShare(ElGamalCipher cipher, DHPrivateKeyParameters privateKey, BigInteger p);

        int CombineShares(IList<BigInteger> shares, BigInteger d, BigInteger p, BigInteger g);
    }
}
