using System;

namespace Reclient.Authentication.Configuration
{
    public class TokenCreatorConfiguration
    {
        public string UserPrincipalName { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scopes { get; set; }

        public Guid TenantId { get; set; }
    }
}
