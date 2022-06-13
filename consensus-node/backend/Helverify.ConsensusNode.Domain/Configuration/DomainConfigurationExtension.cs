using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Helverify.ConsensusNode.Domain.Model;

[assembly: InternalsVisibleTo("Helverify.ConsensusNode.Domain.Tests")]
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
