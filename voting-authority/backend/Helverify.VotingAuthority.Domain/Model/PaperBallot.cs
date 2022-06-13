using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helverify.VotingAuthority.Domain.Model
{
    internal class PaperBallot
    {
        public string BallotId { get; set; }
        public IList<EncryptedBallot> EncryptedBallots { get; set; } = new List<EncryptedBallot>(2);

        public IList<BallotOption> Options = new List<BallotOption>();
    }

    internal class BallotOption
    {
        public string Name { get; set; }
        public string ShortCode1 { get; set; }
        public string ShortCode2 { get; set; }
    }
}
