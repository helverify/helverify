using Helverify.VotingAuthority.DataAccess.Dao;

namespace Helverify.VotingAuthority.DataAccess.Database;

public interface IMongoService<T> where T : IEntity
{
    Task<List<T>> GetAsync();
    Task<T> GetAsync(string id);
    Task CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task RemoveAsync(string id);
}