using Helverify.VotingAuthority.Domain.Helper;
using Helverify.VotingAuthority.Domain.Model.Virtual;

namespace Helverify.VotingAuthority.Domain.Model.Paper
{
    public class PaperBallot
    {
        public Election Election { get; }
        public string BallotId { get; set; }
        public IList<Ballot> EncryptedBallots { get; set; } = new List<Ballot>(2);

        public IList<PaperBallotOption> Options = new List<PaperBallotOption>();

        public PaperBallot(Election election, Ballot ballot1, Ballot ballot2)
        {
            Election = election;

            EncryptedBallots.Add(ballot1);
            EncryptedBallots.Add(ballot2);

            BallotId = HashHelper.Hash(ballot1.Code, ballot2.Code);

            SetUpShortCodes(ballot1, ballot2);
        }
        private void SetUpShortCodes(Ballot ballot1, Ballot ballot2)
        {
            for (int i = 0; i < ballot1.PlainTextOptions.Count; i++)
            {
                PlainTextOption option1 = ballot1.PlainTextOptions[i];
                PlainTextOption option2 = ballot2.PlainTextOptions[i];

                string shortCode1 = option1.ShortCode;
                string shortCode2 = option2.ShortCode;

                string name = option1.Name == option2.Name
                    ? option1.Name
                    : throw new Exception("Ballot options do not match.");

                Options.Add(new PaperBallotOption(name, shortCode1, shortCode2));
            }
        }
    }
}
