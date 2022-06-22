namespace Helverify.VotingAuthority.DataAccess.Ipfs;

/// <summary>
/// Stores and retrieves data (objects) to and from IPFS.
/// </summary>
public interface IStorageClient
{
    /// <summary>
    /// Stores an object on IPFS
    /// </summary>
    /// <typeparam name="T">Type of the object to store</typeparam>
    /// <param name="obj">Subject of storage</param>
    /// <returns></returns>
    Task<string> Store<T>(T obj);

    /// <summary>
    /// Retrieves an object from IPFS
    /// </summary>
    /// <typeparam name="T">Type of the object to retrieve</typeparam>
    /// <param name="id">CID</param>
    /// <returns></returns>
    Task<T> Retrieve<T>(string id);
}