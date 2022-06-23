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
            string connectionString = Environment.GetEnvironmentVariable("MongoDbConnectionString") ?? throw new InvalidOperationException();
            string ipfsHost = Environment.GetEnvironmentVariable("IpfsHost") ?? throw new InvalidOperationException();
            string web3ConnectionString = Environment.GetEnvironmentVariable("GethEndpoint") ?? throw new ArgumentNullException("GethEndpoint");
            string accountPassword = Environment.GetEnvironmentVariable("BC_ACCOUNT_PWD") ??
                                     throw new ArgumentNullException("BC_ACCOUNT_PWD");

            services.AddHttpClient();
            services.AddSingleton<IRestClient, RestClient>();
            services.AddSingleton<IStorageClient, StorageClient>();
            services.AddSingleton(cfg => new IpfsClient(ipfsHost));
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IWeb3Loader>(cfg =>
            {
                IWeb3Loader loader =
                    new Web3Loader(cfg.GetService<IFileSystem>(), web3ConnectionString, accountPassword);

                loader.LoadInstance();

                return loader;
            });
            services.AddScoped<IMongoClient>(_ => new MongoClient(connectionString));
            services.AddScoped(typeof(IMongoService<>), typeof(MongoService<>));
            

            return services;
        }
    }
}
