namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Contains the data needed for printing a ballot on paper.
    /// </summary>
    public class PrintBallotDto
    {
        /// <summary>
        /// Ballot identifier
        /// </summary>
        public string BallotId { get; set; }

        /// <summary>
        /// Selectable election options / candidates.
        /// </summary>
        public IList<PrintOptionDto> Options { get; set; } = new List<PrintOptionDto>();
    }
}
