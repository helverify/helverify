namespace Helverify.VotingAuthority.Backend.Dto
{
    public class ElectionDto
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public string Question { get; set; }

        public IList<ElectionOptionDto> Options { get; set; }

        public string P { get; set; }

        public string G { get; set; }

        public string? PublicKey { get; set; }
    }
}
