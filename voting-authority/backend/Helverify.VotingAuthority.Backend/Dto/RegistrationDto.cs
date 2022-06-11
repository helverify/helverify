namespace Helverify.VotingAuthority.Backend.Dto
{
    public class RegistrationDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public Uri Endpoint { get; set; }
        public string? ElectionId { get; set; }
        public string? PublicKey { get; set; }
    }
}
