using Microsoft.Extensions.Options;
using Reclient.Authentication.Configuration;
using Reclient.Authentication.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Reclient.Authentication.Handlers
{
    public class ServiceToServiceAuthenticationMessageHandler
        : DelegatingHandler
    {
        private readonly ITokenCreator tokenCreator;
        private readonly TokenCreatorConfiguration tokenCreatorConfiguration;

        public ServiceToServiceAuthenticationMessageHandler(ITokenCreator tokenCreator, IOptions<TokenCreatorConfiguration> options)
        {
            this.tokenCreator = tokenCreator ?? throw new ArgumentNullException(nameof(tokenCreator));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            tokenCreatorConfiguration = options.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancelToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var httpRequestHeaders = request.Headers;

            // If you have the following attribute in your interface, the authorization header will be "Bearer", not null.
            // [Headers("Authorization: Bearer")]
            var authenticationHeaderValue = httpRequestHeaders.Authorization;

            if (authenticationHeaderValue != null)
            {
                var scopes = tokenCreatorConfiguration.Scopes.Split(' ');
                var accessToken = await tokenCreator.GetAccessTokenAsync(scopes).ConfigureAwait(false);
                httpRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationHeaderValue.Scheme, accessToken);
            }

            return await base.SendAsync(request, cancelToken).ConfigureAwait(false);
        }
    }
}
