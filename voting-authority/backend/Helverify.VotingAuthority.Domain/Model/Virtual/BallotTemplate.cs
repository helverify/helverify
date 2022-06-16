using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Domain.Model.Virtual
{
    public class BallotTemplate
    {
        public Election Election { get; set; }

        public IList<PlainTextOption> PlainTextOptions { get; } = new List<PlainTextOption>();

        public BallotTemplate(Election election)
        {
            Election = election;

            int totalNumberOfOptions = election.Options.Count;

            for (int i = 0; i < totalNumberOfOptions; i++)
            {
                // nice trick: https://stackoverflow.com/questions/19237788/creating-a-list-of-given-size-all-initialized-to-some-value-in-c-sharp
                IList<int> option = new List<int>(Enumerable.Repeat(0, totalNumberOfOptions));

                option[i] = 1;

                PlainTextOptions.Add(new PlainTextOption(election.Options[i].Name, option));
            }
        }

        public Ballot Encrypt(DHPublicKeyParameters publicKey)
        {
            return new Ballot(PlainTextOptions, publicKey);
        }
    }
}
