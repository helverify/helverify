using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Helverify.ConsensusNode.Domain.Model;
using Helverify.Cryptography.Encryption;

namespace Helverify.ConsensusNode.Domain.Configuration
{
    public static class DomainConfigurationExtension
    {
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddScoped<IKeyPairHandler, KeyPairHandler>();
            services.AddScoped<ICliRunner, CliRunner>();
            
            return services;
        }
    }
}
