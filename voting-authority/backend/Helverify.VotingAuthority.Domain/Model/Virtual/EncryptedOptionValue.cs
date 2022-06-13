using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Model.Virtual;

public class EncryptedOptionValue
{
    public ElGamalCipher Cipher { get; }
    public ProofOfZeroOrOne ProofOfZeroOrOne { get; }

    public EncryptedOptionValue(int option, DHPublicKeyParameters publicKey)
    {
        IElGamal elGamal = new ExponentialElGamal();

        Cipher = elGamal.Encrypt(option, publicKey, new BigInteger(32, 10, new Random(DateTime.Now.Millisecond)));

        ProofOfZeroOrOne = ProofOfZeroOrOne.Create(option, Cipher.C, Cipher.D, publicKey.Y, Cipher.R,
            publicKey.Parameters.P, publicKey.Parameters.G);
    }

    public bool IsValid(DHPublicKeyParameters publicKey)
    {
        return ProofOfZeroOrOne.Verify(Cipher.C, Cipher.D, publicKey.Y, publicKey.Parameters.P, publicKey.Parameters.G);
    }
}