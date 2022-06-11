namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Represents a single option in an election or vote.
    /// </summary>
    public class ElectionOptionDto
    {
        /// <summary>
        /// Candidate/Option name
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
