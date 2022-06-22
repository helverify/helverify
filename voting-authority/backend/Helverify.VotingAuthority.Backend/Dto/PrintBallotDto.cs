namespace Helverify.VotingAuthority.Backend.Dto
{
    public class PrintBallotDto
    {
        public string BallotId { get; set; }

        public IList<PrintOptionDto> Options { get; set; } = new List<PrintOptionDto>();
    }
}
