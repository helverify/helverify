using System.Runtime.CompilerServices;
using Helverify.VotingAuthority.DataAccess.Configuration;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Repository.Mapping;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Helverify.VotingAuthority.Domain.Tests")]
namespace Helverify.VotingAuthority.Domain.Configuration
{
    /// <summary>
    /// Configuration extension for the Domain layer.
    /// </summary>
    public static class DomainConfigurationExtension
    {
        /// <summary>
        /// Registers all services exposed by the Domain layer.
        /// </summary>
        /// <param name="services">ServiceCollection for DI</param>
        /// <returns></returns>
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddTransient<GenesisConverter>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ElectionProfile>();
                cfg.AddProfile<RegistrationProfile>();
                cfg.AddProfile<GenesisProfile>();
                cfg.AddProfile<BallotProfile>();
            });
            services.AddDataAccessConfiguration();
            services.AddSingleton<IConsensusNodeService, ConsensusNodeService>();
            services.AddScoped<IRepository<Election>, ElectionRepository>();
            services.AddScoped<IRepository<Registration>, RegistrationRepository>();
            services.AddScoped<ICliRunner, CliRunner>();
            services.AddScoped<IBlockchainSetup, BlockchainSetup>();

            return services;
        }
    }
}
