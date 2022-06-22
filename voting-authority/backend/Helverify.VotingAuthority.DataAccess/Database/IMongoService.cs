using Helverify.VotingAuthority.DataAccess.Dao;
using MongoDB.Driver;

namespace Helverify.VotingAuthority.DataAccess.Database;

/// <summary>
/// Provides access to a MongoDB database.
/// </summary>
/// <typeparam name="T">Dao type (must implement IEntity)</typeparam>
public interface IMongoService<T> where T : IEntity
{
    /// <summary>
    /// Returns a list of all entries.
    /// </summary>
    /// <returns>All entries</returns>
    Task<List<T>> GetAsync();

    /// <summary>
    /// Returns a specific entry by its id.
    /// </summary>
    /// <param name="id">Unique identifier</param>
    /// <returns>Entry</returns>
    Task<T> GetAsync(string id);

    /// <summary>
    /// Persists a new entry to the database.
    /// </summary>
    /// <param name="entity">Entry to be persisted.</param>
    /// <returns></returns>
    Task CreateAsync(T entity);

    /// <summary>
    /// Updates a specific entry in the database.
    /// </summary>
    /// <param name="id">Unique identifier</param>
    /// <param name="entity">Entry to be updated.</param>
    /// <returns></returns>
    Task UpdateAsync(string id, T entity);

    /// <summary>
    /// Deletes a specific entry from the database.
    /// </summary>
    /// <param name="id">Unique identifier</param>
    /// <returns></returns>
    Task RemoveAsync(string id);

    /// <summary>
    /// Returns the whole collection for this type.
    /// </summary>
    IMongoCollection<T> Collection { get; }
}