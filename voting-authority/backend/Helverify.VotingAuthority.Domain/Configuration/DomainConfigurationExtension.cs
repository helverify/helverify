using Helverify.VotingAuthority.DataAccess.Configuration;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Repository.Mapping;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Helverify.VotingAuthority.Domain.Configuration
{
    public static class DomainConfigurationExtension
    {
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ElectionProfile>();
            });
            services.AddDataAccessConfiguration();
            services.AddSingleton<IConsensusNodeService, ConsensusNodeService>();
            services.AddScoped<IElectionRepository, ElectionRepository>();

            return services;
        }
    }
}
