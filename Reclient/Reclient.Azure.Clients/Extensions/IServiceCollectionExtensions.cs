using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Reclient.Authentication.Extensions;
using Reclient.Authentication.Handlers;
using Reclient.Azure.Clients.Configuration;
using Reclient.Azure.Clients.Handlers;
using Reclient.Azure.Clients.ResourceGroups;
using Refit;
using System;

namespace Reclient.Azure.Clients.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddClientDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var clientConfiguration = new ClientConfiguration();
            configuration.Bind("ClientConfiguration", clientConfiguration);
            serviceCollection.Configure<ClientConfiguration>(cc => configuration.Bind("ClientConfiguration", cc));
            serviceCollection.AddTokenCreatorDependencies(configuration);

            var asyncRetryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempts => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempts)));

            serviceCollection.AddSingleton<HttpLoggingHandler>();
            serviceCollection.AddSingleton<ServiceToServiceAuthenticationMessageHandler>();

            serviceCollection.AddRefitClient<IResourceGroupClient>()
                .AddPolicyHandler(asyncRetryPolicy)
                .AddHttpMessageHandler<ServiceToServiceAuthenticationMessageHandler>()
                .AddHttpMessageHandler<HttpLoggingHandler>()
                .ConfigureHttpClient(http => http.BaseAddress = clientConfiguration.ApiServiceUri);

            return serviceCollection;
        }
    }
}
