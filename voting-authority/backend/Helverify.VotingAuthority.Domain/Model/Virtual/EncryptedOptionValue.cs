using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Model.Virtual;

/// <summary>
/// Represents one encrypted value of an option.
/// </summary>
public sealed class EncryptedOptionValue
{
    /// <summary>
    /// ElGamal ciphertext
    /// </summary>
    public ElGamalCipher Cipher { get; internal set; }
    
    /// <summary>
    /// Proof that the ciphertext either contains the value zero or one.
    /// </summary>
    public ProofOfZeroOrOne ProofOfZeroOrOne { get; internal set; }


    public EncryptedOptionValue()
    {
        
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="option">Plain text value of the option (= 0 or 1)</param>
    /// <param name="publicKey">Public key of the election</param>
    public EncryptedOptionValue(int option, DHPublicKeyParameters publicKey)
    {
        IElGamal elGamal = new ExponentialElGamal();

        Cipher = elGamal.Encrypt(option, publicKey, new BigInteger(32, 10, new Random(DateTime.Now.Millisecond)));

        ProofOfZeroOrOne = ProofOfZeroOrOne.Create(option, Cipher.C, Cipher.D, publicKey.Y, Cipher.R,
            publicKey.Parameters.P, publicKey.Parameters.G);
    }

    /// <summary>
    /// Verifies the proof that the ciphertext contains either the value zero or one.
    /// </summary>
    /// <param name="publicKey">Public key of the election</param>
    /// <returns>True if the proof is valid, false otherwise</returns>
    public bool IsValid(DHPublicKeyParameters publicKey)
    {
        return ProofOfZeroOrOne.Verify(Cipher.C, Cipher.D, publicKey.Y, publicKey.Parameters.P, publicKey.Parameters.G);
    }
}