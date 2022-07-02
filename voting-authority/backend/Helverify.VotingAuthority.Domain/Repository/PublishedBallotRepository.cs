using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Ipfs;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Repository
{
    internal class PublishedBallotRepository : IPublishedBallotRepository
    {
        private readonly IMapper _mapper;
        private readonly IStorageClient _storageClient;

        public PublishedBallotRepository(IMapper mapper, IStorageClient storageClient)
        {
            _mapper = mapper;
            _storageClient = storageClient;
        }

        public VirtualBallot StoreVirtualBallot(VirtualBallot ballot)
        {
            VirtualBallotDao ballotDao = _mapper.Map<VirtualBallotDao>(ballot);

            string cid = _storageClient.Store(ballotDao).Result;

            ballot.IpfsCid = cid;

            return ballot;
        }

        public string StoreSpoiltBallot(VirtualBallot virtualBallot, IDictionary<string, IList<BigInteger>> randomness)
        {
            SpoiltBallotDao spoiltBallot = _mapper.Map<SpoiltBallotDao>(virtualBallot);

            spoiltBallot.SetRandomness(randomness);

            string cid = _storageClient.Store(spoiltBallot).Result;

            return cid;
        }

        public VirtualBallot RetrieveVirtualBallot(string ipfsCid)
        {
            VirtualBallotDao encryption = _storageClient.Retrieve<VirtualBallotDao>(ipfsCid).Result;

            VirtualBallot virtualBallot = _mapper.Map<VirtualBallot>(encryption);

            return virtualBallot;
        }
    }
}
