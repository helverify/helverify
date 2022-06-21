using System.IO.Abstractions;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.DataAccess.Rest;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Helverify.VotingAuthority.DataAccess.Configuration
{
    /// <summary>
    /// Configuration extension for the DataAccess layer.
    /// </summary>
    public static class DataAccessConfigurationExtension
    {
        /// <summary>
        /// Registers all services exposed by the DataAccess layer.
        /// </summary>
        /// <param name="services">ServiceCollection for DI</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IServiceCollection AddDataAccessConfiguration(this IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("MongoDbConnectionString") ?? throw new InvalidOperationException();
            
            services.AddHttpClient();
            services.AddSingleton<IRestClient, RestClient>();

            services.AddScoped<IMongoClient>(_ => new MongoClient(connectionString));
            services.AddScoped(typeof(IMongoService<>), typeof(MongoService<>));
            services.AddScoped<IFileSystem, FileSystem>();
            
            return services;
        }
    }
}
