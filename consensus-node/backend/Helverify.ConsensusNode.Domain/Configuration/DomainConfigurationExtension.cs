using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using Helverify.ConsensusNode.DataAccess.Configuration;
using Helverify.ConsensusNode.Domain.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Helverify.ConsensusNode.Domain.Model;
using Helverify.ConsensusNode.Domain.Repository;

[assembly: InternalsVisibleTo("Helverify.ConsensusNode.Domain.Tests")]
namespace Helverify.ConsensusNode.Domain.Configuration
{
    /// <summary>
    /// Extension class to register domain services.
    /// </summary>
    public static class DomainConfigurationExtension
    {
        /// <summary>
        /// Adds all services needed by the domain layer to the DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDataAccessConfiguration();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<BallotEncryptionProfile>();
            });
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ICliRunner, CliRunner>();
            services.AddScoped<IKeyPairHandler, KeyPairHandler>();
            services.AddScoped<IBallotRepository, BallotRepository>();

            return services;
        }
    }
}
