namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Election statistics
    /// </summary>
    public struct ElectionStatisticsDto
    {
        /// <summary>
        /// Number of ballots created in total
        /// </summary>
        public int NumberOfBallotsTotal { get; set; }

        /// <summary>
        /// Number of ballots already cast
        /// </summary>
        public int NumberOfBallotsCast { get; set; }
    }
}
