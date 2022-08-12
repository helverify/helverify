namespace Helverify.VotingAuthority.Domain.Model
{
    /// <summary>
    /// Election statistics
    /// </summary>
    public sealed class ElectionNumbers
    {
        /// <summary>
        /// Number of ballots generated for this election
        /// </summary>
        public int NumberOfBallotsTotal { get; }

        /// <summary>
        /// Number of ballots already cast for this election
        /// </summary>
        public int NumberOfBallotsCast { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numberOfBallotsTotal">Number of ballots generated for this election</param>
        /// <param name="numberOfBallotsCast">Number of ballots already cast for this election</param>
        public ElectionNumbers(int numberOfBallotsTotal, int numberOfBallotsCast)
        {
            NumberOfBallotsTotal = numberOfBallotsTotal;
            NumberOfBallotsCast = numberOfBallotsCast;
        }
    }
}
