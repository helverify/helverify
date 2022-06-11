using Helverify.VotingAuthority.DataAccess.Configuration;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Helverify.VotingAuthority.Domain.Configuration
{
    public static class DomainConfigurationExtension
    {
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddDataAccessConfiguration();
            services.AddSingleton<IConsensusNodeService, ConsensusNodeService>();

            return services;
        }
    }
}
