using Helverify.VotingAuthority.Domain.Model.Decryption;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Repository;

/// <summary>
/// Provides access to published ballots
/// </summary>
public interface IPublishedBallotRepository
{
    /// <summary>
    /// Stores a virtual ballot on IPFS
    /// </summary>
    /// <param name="ballot">Ballot to be stored</param>
    /// <returns></returns>
    VirtualBallot StoreVirtualBallot(VirtualBallot ballot);

    /// <summary>
    /// Stores a spoilt ballot on IPFS
    /// </summary>
    /// <param name="virtualBallot">Ballot to be stored</param>
    /// <param name="randomness">Randomness used to encrypt ballot</param>
    /// <returns></returns>
    string StoreSpoiltBallot(VirtualBallot virtualBallot, IDictionary<string, IList<BigInteger>> randomness);

    /// <summary>
    /// Retrieves a stored ballot from IPFS
    /// </summary>
    /// <param name="ipfsCid">IPFS cid (address) of the ballot</param>
    /// <returns></returns>
    VirtualBallot RetrieveVirtualBallot(string ipfsCid);

    /// <summary>
    /// Stores the decrypted results on IPFS
    /// </summary>
    /// <param name="decryptedValues">Decrypted data for verification</param>
    /// <returns>IPFS cid of the evidence.</returns>
    string StoreDecryptedResults(IList<DecryptedValue> decryptedValues);
}