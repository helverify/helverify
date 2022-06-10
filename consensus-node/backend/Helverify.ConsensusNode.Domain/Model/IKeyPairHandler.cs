using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;

namespace Helverify.ConsensusNode.Domain.Model;

public interface IKeyPairHandler
{
    AsymmetricCipherKeyPair CreateKeyPair(BigInteger p, BigInteger g);
    ProofOfPrivateKeyOwnership GeneratePrivateKeyProof(AsymmetricCipherKeyPair keyPair);
    void SaveToDisk(AsymmetricCipherKeyPair keyPair);
    AsymmetricCipherKeyPair LoadFromDisk();
}