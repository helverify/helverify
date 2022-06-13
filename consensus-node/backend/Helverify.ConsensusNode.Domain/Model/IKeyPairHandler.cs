using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;

namespace Helverify.ConsensusNode.Domain.Model;

/// <summary>
/// Provides life cycle operations for ElGamal key pairs.
/// </summary>
public interface IKeyPairHandler
{
    /// <summary>
    /// Create a new ElGamal key pair.
    /// </summary>
    /// <param name="p">Public prime p of the ElGamal cryptosystem</param>
    /// <param name="g">Generator g of the ElGamal cryptosystem</param>
    /// <returns></returns>
    AsymmetricCipherKeyPair CreateKeyPair(BigInteger p, BigInteger g);

    /// <summary>
    /// Generates a Schnorr-Proof of private key ownership.
    /// </summary>
    /// <param name="keyPair">ElGamal key pair</param>
    /// <returns></returns>
    ProofOfPrivateKeyOwnership GeneratePrivateKeyProof(AsymmetricCipherKeyPair keyPair);

    /// <summary>
    /// Saves the specified key pair to disk in PEM format.
    /// </summary>
    /// <param name="keyPair">ElGamal key pair</param>
    void SaveToDisk(AsymmetricCipherKeyPair keyPair);

    /// <summary>
    /// Loads the stored ElGamal key pair from disk.
    /// </summary>
    /// <returns>ElGamal key pair</returns>
    AsymmetricCipherKeyPair LoadFromDisk();
}