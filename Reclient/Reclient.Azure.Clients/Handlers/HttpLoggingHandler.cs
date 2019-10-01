using Microsoft.Extensions.Logging;
using Reclient.Azure.Clients.Resources;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Reclient.Azure.Clients.Handlers
{
    public class HttpLoggingHandler
        : DelegatingHandler
    {
        private readonly ILogger<HttpLoggingHandler> logger;

        public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
        {
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var start = DateTime.Now;
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var end = DateTime.Now;

            logger.LogDebug(ClientResources.LineSeparator);
            logger.LogDebug(ClientResources.HttpClientRequestMessage, request.Method, request.RequestUri.PathAndQuery, request.RequestUri.Scheme, request.Version);
            logger.LogDebug(ClientResources.DurationMessage, end - start);

            foreach (var kvp in request.Headers)
            {
                logger.LogDebug($"{kvp.Key}: {string.Join(", ", kvp.Value)}");
            }

            if (request.Content != null)
            {
                var requestContent = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(requestContent))
                {
                    logger.LogDebug(ClientResources.ContentMessage);
                    logger.LogDebug($"{string.Join(string.Empty, requestContent?.Take(255))}...");
                }
            }

            logger.LogDebug(ClientResources.HttpClientResponseMessage, request.RequestUri.Scheme.ToUpper(CultureInfo.CurrentCulture), response.Version, (int)response.StatusCode, response.ReasonPhrase);
            foreach (var kvp in response.Headers)
            {
                logger.LogDebug($"{kvp.Key}: {string.Join(", ", kvp.Value)}");
            }

            if (response.Content != null)
            {
                var responseContent = await (response.Content?.ReadAsStringAsync()).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    logger.LogDebug(ClientResources.ContentMessage);
                    logger.LogDebug($"{string.Join(string.Empty, responseContent?.Take(255))}...");
                }
            }

            return response;
        }
    }
}
