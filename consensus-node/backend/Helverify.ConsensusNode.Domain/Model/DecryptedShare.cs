using Helverify.Cryptography.ZeroKnowledge;

namespace Helverify.ConsensusNode.Domain.Model
{
    public class DecryptedShare
    {
        public string Share { get; set; }
        public ProofOfDecryption ProofOfDecryption { get; set; }
    }
}
