using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository;

public interface IElectionRepository
{
    Task<Election> CreateAsync(Election election);
    Task<Election> GetAsync(string id);
    Task<IList<Election>> GetAsync();
    Task<Election> UpdateAsync(string id, Election election);
    Task DeleteAsync(string id);
}