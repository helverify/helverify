namespace Helverify.ConsensusNode.DataAccess.Ipfs;

public interface IStorageClient
{
    /// <summary>
    /// Retrieves an object from IPFS
    /// </summary>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <param name="id">IPFS Cid</param>
    /// <returns></returns>
    Task<T> Retrieve<T>(string id);
}