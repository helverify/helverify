namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Represents a registered consensus node.
    /// </summary>
    public class RegistrationDto
    {
        /// <summary>
        /// Name of the consensus node.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// API Endpoint for calling the consensus node.
        /// </summary>
        public Uri? Endpoint { get; set; }
        
        /// <summary>
        /// Public key of the consensus node.
        /// </summary>
        public IList<string> PublicKeys { get; set; } = new List<string>();
    }
}
