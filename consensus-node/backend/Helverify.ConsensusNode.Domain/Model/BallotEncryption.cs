namespace Helverify.ConsensusNode.Domain.Model
{
    public class BallotEncryption
    {
        public BallotEncryption(IDictionary<string, IList<CipherText>> encryptions)
        {
            Encryptions = encryptions;
        }

        public IDictionary<string, IList<CipherText>> Encryptions { get; }
    }
}
