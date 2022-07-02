using Helverify.VotingAuthority.Domain.Model.Virtual;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Repository;

public interface IPublishedBallotRepository
{
    VirtualBallot StoreVirtualBallot(VirtualBallot ballot);

    string StoreSpoiltBallot(VirtualBallot virtualBallot, IDictionary<string, IList<BigInteger>> randomness);

    VirtualBallot RetrieveVirtualBallot(string ipfsCid);
}