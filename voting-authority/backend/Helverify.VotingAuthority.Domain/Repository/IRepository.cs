namespace Helverify.VotingAuthority.Domain.Repository;

/// <summary>
/// Provides access to domain objects of type T
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T>
{
    /// <summary>
    /// Creates a new domain object instance.
    /// </summary>
    /// <param name="entity">Entity to be created</param>
    /// <returns>Created instance</returns>
    Task<T> CreateAsync(T entity);

    /// <summary>
    /// Retrieves a specific domain object instance.
    /// </summary>
    /// <param name="id">Unique identifier of the entity</param>
    /// <returns>Specific instance</returns>
    Task<T> GetAsync(string id);

    /// <summary>
    /// Retrieves all domain object instances of the defined type T.
    /// </summary>
    /// <returns>All instances</returns>
    Task<IList<T>> GetAsync();

    /// <summary>
    /// Updates a specific domain object instance.
    /// </summary>
    /// <param name="id">Unique identifier of the entity</param>
    /// <param name="entity">Entity to be updated</param>
    /// <returns>Updated instance</returns>
    Task<T> UpdateAsync(string id, T entity);

    /// <summary>
    /// Removes a specific domain object instance.
    /// </summary>
    /// <param name="id">Unique identifier of the entity</param>
    /// <returns></returns>
    Task DeleteAsync(string id);
}