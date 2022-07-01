using Helverify.ConsensusNode.Domain.Model;

namespace Helverify.ConsensusNode.Domain.Repository;

public interface IBallotRepository
{
    Task<BallotEncryption> GetBallotEncryption(string cid);
}