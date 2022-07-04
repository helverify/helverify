using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.DataAccess.Dto
{
    public class DecryptedBallotShareDto
    {
        public BigInteger PublicKey { get; set; }
        public IDictionary<string, IList<DecryptionShareDto>> DecryptedShares { get; set; } = new Dictionary<string, IList<DecryptionShareDto>>();
    }
}
