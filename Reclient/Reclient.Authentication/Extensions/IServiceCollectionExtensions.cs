using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reclient.Authentication.Configuration;
using Reclient.Authentication.Interfaces;
using System;

namespace Reclient.Authentication.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenCreatorDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            serviceCollection.Configure<TokenCreatorConfiguration>(tcc => configuration.Bind("TokenCreator", tcc));
            serviceCollection.AddSingleton<ITokenCreator, UserTokenCreator>();

            return serviceCollection;
        }
    }
}
