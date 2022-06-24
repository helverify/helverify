namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Contains the parameters for ballot generation
    /// </summary>
    public class BallotGenerationDto
    {
        /// <summary>
        /// Number of ballots to be generated at once.
        /// </summary>
        public int NumberOfBallots { get; set; }
    }
}
