using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Domain.Model.Virtual;

public class SumProof
{
    public ProofOfContainingOne Proof { get; }
    public ElGamalCipher Cipher { get; }

    public SumProof(ProofOfContainingOne proof, ElGamalCipher cipher)
    {
        Proof = proof;
        Cipher = cipher;
    }

    public bool IsValid(DHPublicKeyParameters publicKey)
    {
        return Proof.Verify(Cipher.C, Cipher.D, publicKey.Y, publicKey.Parameters.P, publicKey.Parameters.G);
    }
}