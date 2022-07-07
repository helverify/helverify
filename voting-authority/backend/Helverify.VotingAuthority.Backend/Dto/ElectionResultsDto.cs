using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Represents an election's final tally.
    /// </summary>
    public struct ElectionResultsDto
    {
        /// <summary>
        /// Tally per candidate / option
        /// </summary>
        public IList<ElectionResultDto> Results { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ElectionResultsDto()
        {
            Results = new List<ElectionResultDto>();
        }
    }
}
