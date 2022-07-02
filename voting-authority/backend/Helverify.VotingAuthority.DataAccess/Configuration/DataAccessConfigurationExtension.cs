using System.IO.Abstractions;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.DataAccess.Ethereum;
using Helverify.VotingAuthority.DataAccess.Ipfs;
using Helverify.VotingAuthority.DataAccess.Rest;
using Ipfs.Http;
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
            string connectionString = Environment.GetEnvironmentVariable(EnvironmentVariables.MongoDbConnectionString) ?? throw new InvalidOperationException();
            string ipfsHost = Environment.GetEnvironmentVariable(EnvironmentVariables.IpfsHost) ?? throw new InvalidOperationException();
            string accountPassword = Environment.GetEnvironmentVariable(EnvironmentVariables.BcAccountPassword) ??
                                     throw new ArgumentNullException(nameof(EnvironmentVariables.BcAccountPassword));

            services.AddHttpClient();
            services.AddSingleton<IRestClient, RestClient>();
            services.AddSingleton<IStorageClient, StorageClient>();
            services.AddSingleton(_ => new IpfsClient(ipfsHost));
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton(cfg =>
            {
                IWeb3Loader loader =
                    new Web3Loader(cfg.GetService<IFileSystem>()!, accountPassword);

                loader.LoadInstance();

                return loader;
            });
            services.AddScoped<IMongoClient>(_ => new MongoClient(connectionString));
            services.AddScoped(typeof(IMongoService<>), typeof(MongoService<>));
            
            return services;
        }
    }
}
