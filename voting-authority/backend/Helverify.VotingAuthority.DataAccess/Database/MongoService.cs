using Helverify.VotingAuthority.DataAccess.Attributes;
using Helverify.VotingAuthority.DataAccess.Dao;
using MongoDB.Driver;

namespace Helverify.VotingAuthority.DataAccess.Database
{
    /// <summary>
    /// Followed this guide: https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-6.0&amp;tabs=visual-studio
    /// </summary>
    internal class MongoService<T> : IMongoService<T> where T: IEntity
    {
        private readonly IMongoCollection<T> _collection;

        public IMongoCollection<T> Collection => _collection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mongoClient">MongoDB accessor</param>
        /// <exception cref="InvalidOperationException"></exception>
        public MongoService(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(Constants.MongoDbDatabaseName);
            
            CollectionNameAttribute collectionNameAttr = typeof(T).GetCustomAttributes(typeof(CollectionNameAttribute), true).First() as CollectionNameAttribute
                                                         ?? throw new InvalidOperationException();
            
            _collection = database.GetCollection<T>(collectionNameAttr.Name);
        }

        /// <inheritdoc cref="IMongoService{T}.GetAsync()"/>
        public async Task<List<T>> GetAsync() => await _collection.Find(_ => true).ToListAsync();

        /// <inheritdoc cref="IMongoService{T}.GetAsync(string {id})"/>
        public async Task<T> GetAsync(string id) => await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();

        /// <inheritdoc cref="IMongoService{T}.CreateAsync"/>
        public async Task CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        /// <inheritdoc cref="IMongoService{T}.UpdateAsync"/>
        public async Task UpdateAsync(string id, T entity)
        {
            await _collection.ReplaceOneAsync(e => e.Id == id, entity);
        }

        /// <inheritdoc cref="IMongoService{T}.RemoveAsync"/>
        public async Task RemoveAsync(string id) => await _collection.DeleteOneAsync(e => e.Id == id);
    }
}
