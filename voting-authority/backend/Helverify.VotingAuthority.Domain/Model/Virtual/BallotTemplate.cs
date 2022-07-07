using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.VotingAuthority.Domain.Model.Virtual
{
    /// <summary>
    /// Template containing all options of the ballot for an election. Defined once per election.
    /// </summary>
    public sealed class BallotTemplate
    {
        /// <summary>
        /// Election for which the options are valid.
        /// </summary>
        public Model.Election Election { get; set; }

        /// <summary>
        /// Contains the plaintext options for the election.
        /// </summary>
        public IList<PlainTextOption> PlainTextOptions { get; } = new List<PlainTextOption>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="election">Election for which this template is valid.</param>
        public BallotTemplate(Model.Election election)
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

        /// <summary>
        /// Encrypts the ballot template, generating a unique virtual ballot.
        /// </summary>
        /// <returns></returns>
        public VirtualBallot Encrypt()
        {
            VirtualBallot virtualBallot = CreateVirtualBallot();

            while (!virtualBallot.AreShortCodesUnique())
            {
                virtualBallot = CreateVirtualBallot();
            }

            return virtualBallot;
        }

        private VirtualBallot CreateVirtualBallot()
        {
            return new VirtualBallot(PlainTextOptions,
                new DHPublicKeyParameters(Election.PublicKey, Election.DhParameters));
        }
    }
}
