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
    public static class DomainConfigurationExtension
    {
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
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
