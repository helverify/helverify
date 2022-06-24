namespace Helverify.VotingAuthority.Domain.Model
{
    /// <summary>
    /// Represents a single option in an election or vote.
    /// </summary>
    public sealed class ElectionOption
    {
        /// <summary>
        /// Candidate/Option name
        /// </summary>
        public string Name { get; set; }
    }
}
