using Helverify.Cryptography.Encryption;
using Helverify.VotingAuthority.Domain.Model.Virtual;

namespace Helverify.VotingAuthority.Domain.Model
{
    /// <summary>
    /// Contains tally calculation logic
    /// </summary>
    public class Tally
    {
        private readonly IList<EncryptedOption> _encryptedOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encryptedOptions">All selected options in encrypted form</param>
        public Tally(IList<EncryptedOption> encryptedOptions)
        {
            _encryptedOptions = encryptedOptions;
        }
        
        /// <summary>
        /// Performs homomorphic addition of the encrypted options to derive the final (encrypted) tally.
        /// </summary>
        /// <param name="election">Current election</param>
        /// <returns></returns>
        public IList<ElGamalCipher> CalculateCipherResult(Election election)
        {
            IList<ElGamalCipher> optionsVector = new List<ElGamalCipher>();

            for (int i = 0; i < _encryptedOptions.Count; i++)
            {
                if (i == 0)
                {
                    optionsVector = _encryptedOptions[i].Values.Select(v => v.Cipher).ToList();
                    continue;
                }

                for (int j = 0; j < optionsVector.Count; j++)
                {
                    optionsVector[j] = optionsVector[j].Add(_encryptedOptions[i].Values[j].Cipher, election.P);
                }
            }

            return optionsVector;
        }
    }
}
