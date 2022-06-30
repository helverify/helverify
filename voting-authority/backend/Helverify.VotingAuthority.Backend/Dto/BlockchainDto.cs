namespace Helverify.VotingAuthority.Backend.Dto
{
    public struct BlockchainDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public IList<RegistrationDto> Registrations { get; set; }
    }
}
