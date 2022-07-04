namespace Helverify.ConsensusNode.Domain.Model
{
    /// <summary>
    /// Represents an encrypted ballot
    /// </summary>
    public class BallotEncryption
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encryptions">Dictionary of encryptions with short code as key</param>
        public BallotEncryption(IDictionary<string, IList<CipherText>> encryptions)
        {
            Encryptions = encryptions;
        }

        /// <summary>
        /// Dictionary of encryptions with short code as key
        /// </summary>
        public IDictionary<string, IList<CipherText>> Encryptions { get; }
    }
}
