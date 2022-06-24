using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Domain.Model.Virtual;

/// <summary>
/// Represents a proof that a ciphertext (result of a sum of ciphertext) contains the value one.
/// </summary>
public sealed class SumProof
{
    /// <summary>
    /// Proof that the ciphertext contains the value one.
    /// </summary>
    public ProofOfContainingOne Proof { get; }

    /// <summary>
    /// ElGamal ciphertext, result of summing up the ciphertexts of a row or column.
    /// </summary>
    public ElGamalCipher Cipher { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="proof">Non-Interactive Zero-Knowledge proof of containing one</param>
    /// <param name="cipher">ElGamal ciphertext</param>
    public SumProof(ProofOfContainingOne proof, ElGamalCipher cipher)
    {
        Proof = proof;
        Cipher = cipher;
    }

    /// <summary>
    /// Verifies the proof of containing one.
    /// </summary>
    /// <param name="publicKey">Public key of the election</param>
    /// <returns>True if the proof is valid (i.e., the ciphertext contains the value one), false otherwise</returns>
    public bool IsValid(DHPublicKeyParameters publicKey)
    {
        return Proof.Verify(Cipher.C, Cipher.D, publicKey.Y, publicKey.Parameters.P, publicKey.Parameters.G);
    }
}