namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Contains the blockchain parameters for configuration
    /// </summary>
    public struct BlockchainDto
    {
        /// <summary>
        /// ID of the blockchain
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Caption
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Registered consensus nodes
        /// </summary>
        public IList<RegistrationDto> Registrations { get; set; }
    }
}
