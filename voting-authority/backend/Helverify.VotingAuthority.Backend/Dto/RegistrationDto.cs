namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Represents a registered consensus node.
    /// </summary>
    public class RegistrationDto
    {
        /// <summary>
        /// Unique identifier for the consensus node.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Name of the consensus node.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// API Endpoint for calling the consensus node.
        /// </summary>
        public Uri? Endpoint { get; set; }

        /// <summary>
        /// Identifier of the corresponding election for which the registration is valid.
        /// </summary>
        public string? ElectionId { get; set; }

        /// <summary>
        /// Public key of the consensus node.
        /// </summary>
        public string? PublicKey { get; set; }
    }
}
