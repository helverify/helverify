using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Helverify.ConsensusNode.Domain.Model;

namespace Helverify.ConsensusNode.Domain.Configuration
{
    public static class DomainConfigurationExtension
    {
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IKeyPairHandler, KeyPairHandler>();
            services.AddScoped<ICliRunner, CliRunner>();
            services.AddSingleton<IFileSystem, FileSystem>();

            return services;
        }
    }
}
