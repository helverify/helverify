﻿namespace Helverify.VotingAuthority.DataAccess.Dto
{
    public class DecryptedBallotShareDto
    {
        public IDictionary<string, IList<DecryptionShareDto>> DecryptedShares { get; set; } = new Dictionary<string, IList<DecryptionShareDto>>();
    }
}
