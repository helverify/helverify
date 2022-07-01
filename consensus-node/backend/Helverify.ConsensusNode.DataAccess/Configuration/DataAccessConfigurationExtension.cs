using Helverify.ConsensusNode.DataAccess.Ipfs;
using Ipfs.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Helverify.ConsensusNode.DataAccess.Configuration
{
    public static class DataAccessConfigurationExtension
    {
        public static IServiceCollection AddDataAccessConfiguration(this IServiceCollection services)
        {
            string ipfsHost = Environment.GetEnvironmentVariable("IpfsHost") ?? throw new InvalidOperationException();

            services.AddSingleton(_ => new IpfsClient(ipfsHost));
            services.AddScoped<IStorageClient, StorageClient>();
            
            return services;
        }
    }
}
