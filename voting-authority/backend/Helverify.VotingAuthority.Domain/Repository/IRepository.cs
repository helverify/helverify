namespace Helverify.VotingAuthority.Domain.Repository;

public interface IRepository<T>
{
    Task<T> CreateAsync(T registration);
    Task<T> GetAsync(string id);
    Task<IList<T>> GetAsync();
    Task<T> UpdateAsync(string id, T registration);
    Task DeleteAsync(string id);
}