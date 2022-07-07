using Helverify.VotingAuthority.Domain.Model.Consensus;

namespace Helverify.VotingAuthority.Domain.Model.Blockchain
{
    /// <summary>
    /// Represents a Blockchain for deploying smart contracts
    /// </summary>
    public class Blockchain
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Caption
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Registered consensus nodes
        /// </summary>
        public IList<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
