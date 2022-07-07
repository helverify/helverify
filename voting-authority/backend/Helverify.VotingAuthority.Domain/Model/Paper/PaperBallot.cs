using Helverify.VotingAuthority.Domain.Helper;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Org.BouncyCastle.Math;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Helverify.VotingAuthority.Domain.Model.Paper
{
    /// <summary>
    /// Represents a paper ballot consisting of two virtual ballots.
    /// </summary>
    public sealed class PaperBallot
    {
        /// <summary>
        /// Election in for this paper ballot has been produced.
        /// </summary>
        public Model.Election Election { get; set; }

        /// <summary>
        /// Identifier, consists of the hash of both virtual ballots.
        /// </summary>
        public string BallotId { get; }

        /// <summary>
        /// Contains the two virtual ballots.
        /// </summary>
        public IList<VirtualBallot> Ballots { get; } = new List<VirtualBallot>(2);

        /// <summary>
        /// Contains the ballot options with their corresponding pairs of short codes.
        /// </summary>
        public IList<PaperBallotOption> Options { get; private set; } = new List<PaperBallotOption>();

        public bool Printed { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="election">Corresponding election</param>
        /// <param name="ballot1">First virtual ballot</param>
        /// <param name="ballot2">Second virtual ballot</param>
        public PaperBallot(Model.Election election, VirtualBallot ballot1, VirtualBallot ballot2)
        {
            Election = election;

            Ballots.Add(ballot1);
            Ballots.Add(ballot2);

            HashHelper hashHelper = new HashHelper();

            BallotId = hashHelper.Hash(ballot1.Code, ballot2.Code);

            SetUpShortCodes(ballot1, ballot2);

            Printed = false;
        }

        /// <summary>
        /// Constructor for deserialization
        /// </summary>
        public PaperBallot(string ballotId, IList<PaperBallotOption> options)
        {
            BallotId = ballotId;
            Options = options;
        }

        /// <summary>
        /// Retrieves the random values used to encrypt the ballot.
        /// </summary>
        /// <param name="ballotIndex">Index of the desired ballot (0 or 1)</param>
        /// <returns></returns>
        public IDictionary<string, IList<BigInteger>> GetRandomness(int ballotIndex)
        {
            IDictionary<string, IList<BigInteger>> randomness = new Dictionary<string, IList<BigInteger>>();

            foreach (PaperBallotOption paperBallotOption in Options)
            {
                randomness[paperBallotOption.GetShortCode(ballotIndex)] = paperBallotOption.GetRandomness(ballotIndex);
            }

            return randomness;
        }

        /// <summary>
        /// Generates a PDF of this ballot.
        /// </summary>
        /// <param name="pdfTemplate">PDF template to be used</param>
        /// <returns></returns>
        public byte[] CreatePdf(IDocument pdfTemplate)
        {
            byte[] pdfBytes = pdfTemplate.GeneratePdf();

            Printed = true;

            return pdfBytes;
        }

        /// <summary>
        /// Populates the options by calculating the short codes.
        /// </summary>
        /// <param name="ballot1">First virtual ballot</param>
        /// <param name="ballot2">Second virtual ballot</param>
        /// <exception cref="Exception">Throws if the supplied ballots contain different options, i.e., they are incompatible.</exception>
        private void SetUpShortCodes(VirtualBallot ballot1, VirtualBallot ballot2)
        {
            IDictionary<string, IList<BigInteger>> randomness1 = ballot1.GetRandomness();
            IDictionary<string, IList<BigInteger>> randomness2 = ballot2.GetRandomness();

            for (int i = 0; i < ballot1.PlainTextOptions.Count; i++)
            {
                PlainTextOption option1 = ballot1.PlainTextOptions[i];
                PlainTextOption option2 = ballot2.PlainTextOptions[i];

                string shortCode1 = option1.ShortCode;
                string shortCode2 = option2.ShortCode;
                
                IList<BigInteger> randomValues1 = randomness1[shortCode1];
                IList<BigInteger> randomValues2 = randomness2[shortCode2];

                string name = option1.Name == option2.Name
                    ? option1.Name
                    : throw new Exception("VirtualBallot options do not match.");

                Options.Add(new PaperBallotOption(name, shortCode1, shortCode2, randomValues1, randomValues2));
            }
        }

        /// <summary>
        /// Verifies if the selected short codes are a subset of either of the two sets of short codes contained in this ballot.
        /// </summary>
        /// <param name="selection">Selected short codes</param>
        /// <returns></returns>
        public bool HasShortCodes(IList<string> selection)
        {
            // inspired by https://stackoverflow.com/questions/332973/check-whether-an-array-is-a-subset-of-another
            bool areShortCodesOfBallot1 = selection.All(s => Options.Select(s => s.ShortCode1).Contains(s));
            
            bool areShortCodesOfBallot2 = selection.All(s => Options.Select(s => s.ShortCode2).Contains(s));

            return areShortCodesOfBallot1 || areShortCodesOfBallot2;
        }

        /// <summary>
        /// Removes the plaintext option names from all ballots.
        /// </summary>
        public void ClearConfidential()
        {
            foreach (PaperBallotOption paperBallotOption in Options)
            {
                paperBallotOption.ClearConfidential();
            }

            Options = Options.OrderBy(o => o.ShortCode1).ToList();
        }
    }
}
