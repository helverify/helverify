
using Helverify.VotingAuthority.Application.Services;
using Helverify.VotingAuthority.Domain.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Helverify.VotingAuthority.Application.Configuration
{
    /// <summary>
    /// Configuration extension for the DataAccess layer.
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// Registers all services exposed by the DataAccess layer.
        /// </summary>
        /// <param name="services">ServiceCollection for DI</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            services.AddDomainConfiguration();
            services.AddScoped<IElectionService, ElectionService>();
            services.AddScoped<IBallotService, BallotService>();
            services.AddScoped<IBallotPrintService, BallotPrintService>();
            services.AddScoped<IBlockchainService, BlockchainService>();

            return services;
        }
    
    }
}
