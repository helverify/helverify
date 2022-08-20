using Helverify.ConsensusNode.DataAccess.Ipfs;
using Ipfs.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Helverify.ConsensusNode.DataAccess.Configuration
{
    /// <summary>
    /// Extension class for the data access layer
    /// </summary>
    public static class DataAccessConfigurationExtension
    {
        /// <summary>
        /// Registers all needed services by the data access layer to the DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IServiceCollection AddDataAccessConfiguration(this IServiceCollection services)
        {
            string ipfsHost = Environment.GetEnvironmentVariable("IpfsHost") ?? throw new InvalidOperationException();

            services.AddSingleton(_ => new IpfsClient(ipfsHost));
            services.AddScoped<IStorageClient, StorageClient>();

            return services;
        }
    }
}
