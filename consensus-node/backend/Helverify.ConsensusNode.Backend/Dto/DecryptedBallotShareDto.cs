namespace Helverify.ConsensusNode.Backend.Dto
{
    public class DecryptedBallotShareDto
    {
        public IDictionary<string, IList<DecryptionShareDto>> DecryptedShares { get; set; } = new Dictionary<string, IList<DecryptionShareDto>>();
    }
}
