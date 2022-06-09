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
    /// Elgamal1985
    /// https://github.com/meck93/evote-crypto
    /// https://github.com/meck93/evote-crypto/blob/master/src/ff-elGamal/encryption.ts
    /// https://github.com/bcgit/bc-csharp/
    /// https://nvotes.com/multiplicative-vs-additive-homomorphic-elGamal/
    /// </summary>
    public interface IElGamal
    {
        /// <summary>
        /// Loads a Diffie-Hellman key from the specified path.
        /// Inspired by the following sources:
        /// https://stackoverflow.com/questions/11346200/reading-pem-rsa-public-key-only-using-bouncy-castle
        /// https://stackoverflow.com/questions/15629551/read-rsa-privatekey-in-c-sharp-and-bouncy-castle
        /// </summary>
        /// <param name="path">Path of the PEM file.</param>
        /// <returns>Diffie-Hellman public or private key</returns>
        AsymmetricKeyParameter GetKey(string path);

        /// <summary>
        /// Converts p and g to Diffie-Hellman parameters.
        /// </summary>
        /// <param name="p">Prime</param>
        /// <param name="g">Generator</param>
        /// <returns>Diffie-Hellman parameters</returns>
        DHParameters GetParameters(BigInteger p, BigInteger g);

        /// <summary>
        /// Generates an ElGamal (Diffie-Hellman) key pair.
        /// </summary>
        /// <param name="p">Prime</param>
        /// <param name="g">Generator</param>
        /// <param name="random">Source of randomness</param>
        /// <returns>ElGamal key pair</returns>
        AsymmetricCipherKeyPair KeyGen(BigInteger p, BigInteger g, SecureRandom random = null);

        /// <summary>
        /// Encrypts a message with the given public key.
        /// </summary>
        /// <param name="message">Plaintext message to be encrypted</param>
        /// <param name="publicKeyParameter">Public key</param>
        /// <param name="randomness">Source of randomness</param>
        /// <returns>Ciphertext</returns>
        ElGamalCipher Encrypt(int message, AsymmetricKeyParameter publicKeyParameter, BigInteger randomness = null);

        /// <summary>
        /// Decrypts a ciphertext to retrieve the plaintext message.
        /// </summary>
        /// <param name="cipher">Ciphertext</param>
        /// <param name="privateKeyParameter">Private key</param>
        /// <returns>Plaintext message</returns>
        int Decrypt(ElGamalCipher cipher, AsymmetricKeyParameter privateKeyParameter);

        /// <summary>
        /// Creates a combined public key from all the specified public keys.
        /// </summary>
        /// <param name="publicKeys">Public keys to be combined.</param>
        /// <param name="elGamalParameters">Parameters of the ElGamal cryptosystem.</param>
        /// <returns>Composite public key</returns>
        DHPublicKeyParameters CombinePublicKeys(IList<DHPublicKeyParameters> publicKeys,
            DHParameters elGamalParameters);

        /// <summary>
        /// Decrypts a share of a ciphertext.
        /// </summary>
        /// <param name="cipher">Ciphertext</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="p">Prime</param>
        /// <returns>Decrypted share</returns>
        BigInteger DecryptShare(ElGamalCipher cipher, DHPrivateKeyParameters privateKey, BigInteger p);

        /// <summary>
        /// Combines the decrypted shares to reveal the plaintext message.
        /// </summary>
        /// <param name="shares">Decryption shares</param>
        /// <param name="d">Second component of an ElGamal ciphertext</param>
        /// <param name="p">Prime</param>
        /// <param name="g">Generator</param>
        /// <returns>Plaintext message</returns>
        int CombineShares(IList<BigInteger> shares, BigInteger d, BigInteger p, BigInteger g);
    }
}
