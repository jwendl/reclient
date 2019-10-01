using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Reclient.Authentication.Configuration;
using Reclient.Authentication.Interfaces;
using Reclient.Authentication.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Reclient.Authentication
{
    [ExcludeFromCodeCoverage()]
    public class ClientTokenCreator
        : ITokenCreator
    {
        private readonly IConfidentialClientApplication confidentialClientApplication;
        private readonly ILogger<ClientTokenCreator> logger;

        public ClientTokenCreator(IOptions<TokenCreatorConfiguration> options, ILogger<ClientTokenCreator> logger)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var tokenCreatorConfiguration = options.Value;
            confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(tokenCreatorConfiguration.ClientId)
                .WithClientSecret(tokenCreatorConfiguration.ClientSecret)
                .WithAuthority(AzureCloudInstance.AzurePublic, tokenCreatorConfiguration.TenantId)
                .Build();

            this.logger = logger;
        }

        public async Task<string> GetAccessTokenAsync(IEnumerable<string> scopes)
        {
            try
            {
                var result = await confidentialClientApplication.AcquireTokenForClient(scopes)
                    .ExecuteAsync()
                    .ConfigureAwait(false);
                return result.AccessToken;
            }
            catch (MsalServiceException msalServiceException)
            {
                logger.LogError(msalServiceException, msalServiceException.Message);
                throw new InvalidOperationException(AuthenticationResources.MsalException, msalServiceException);
            }
        }
    }
}
