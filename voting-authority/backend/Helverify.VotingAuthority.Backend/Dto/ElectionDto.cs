namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Represents an Election
    /// </summary>
    public class ElectionDto
    {
        /// <summary>
        /// Unique election identifier.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Name of the election.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Voting/election question
        /// </summary>
        public string Question { get; set; } = string.Empty;

        /// <summary>
        /// Represents the voting options for this election.
        /// </summary>
        public IList<ElectionOptionDto> Options { get; set; } = new List<ElectionOptionDto>();

        /// <summary>
        /// Prime p of the ElGamal cryptosystem.
        /// </summary>
        public string P { get; set; } = string.Empty;

        /// <summary>
        /// Generator g of the ElGamal cryptosystem.
        /// </summary>
        public string G { get; set; } = string.Empty;

        /// <summary>
        /// Election public key
        /// </summary>
        public string? PublicKey { get; set; }

        /// <summary>
        /// Address of the smart contract for the election on the Ethereum blockchain.
        /// </summary>
        public string ContractAddress { get; set; }
    }
}
