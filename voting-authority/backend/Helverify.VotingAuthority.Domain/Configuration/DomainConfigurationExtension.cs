using Helverify.VotingAuthority.DataAccess.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Helverify.VotingAuthority.Domain.Configuration
{
    public static class DomainConfigurationExtension
    {
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddDataAccessConfiguration();

            return services;
        }
    }
}
