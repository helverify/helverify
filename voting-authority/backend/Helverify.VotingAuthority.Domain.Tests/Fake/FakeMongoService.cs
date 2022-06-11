using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;

namespace Helverify.VotingAuthority.Domain.Tests.Fake
{
    internal class FakeMongoService<T>: IMongoService<T> where T : IEntity

    {
        public List<T> Entities = new();
        
        public async Task<List<T>> GetAsync()
        {
            return Entities;
        }

        public async Task<T> GetAsync(string id)
        {
            return Entities.Single(e => e.Id == id);
        }

        public async Task CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            Entities.Add(entity);
        }

        public async Task UpdateAsync(string id, T entity)
        {
            await RemoveAsync(id);

            Entities.Add(entity);
        }

        public async Task RemoveAsync(string id)
        {
            T e = await GetAsync(id);

            Entities.Remove(e);
        }
    }
}
