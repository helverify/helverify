using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Domain.Model
{
    public class BallotTemplate
    {
        public Election Election { get; set; }

        public IList<IList<int>> PlainTextOptions { get; } = new List<IList<int>>();

        public BallotTemplate(Election election)
        {
            Election = election;

            int totalNumberOfOptions = election.Options.Count;

            for(int i = 0; i < totalNumberOfOptions; i++)
            {
                // nice trick: https://stackoverflow.com/questions/19237788/creating-a-list-of-given-size-all-initialized-to-some-value-in-c-sharp
                IList<int> option = new List<int>(Enumerable.Repeat(0, totalNumberOfOptions));
                
                option[i] = 1;

                PlainTextOptions.Add(option);
            }
        }

        public EncryptedBallot Encrypt(DHPublicKeyParameters publicKey)
        {
            return new EncryptedBallot(PlainTextOptions, publicKey);
        }
    }
}
