using Helverify.Cryptography.Encryption;
using Helverify.VotingAuthority.Domain.Model.Virtual;

namespace Helverify.VotingAuthority.Domain.Model
{
    public class Tally
    {
        private readonly IList<EncryptedOption> _encryptedOptions;

        public Tally(IList<EncryptedOption> encryptedOptions)
        {
            _encryptedOptions = encryptedOptions;
        }
        
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
