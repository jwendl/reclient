using Newtonsoft.Json;

namespace Reclient.Azure.Clients.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ResourceGroupRequest
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("properties")]
        public ResourceGroupProperties Properties { get; set; }
    }

    public class ResourceGroup
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("Properties")]
        public ResourceGroupProperties Properties { get; set; }
    }

    public class ResourceGroupProperties
    {
        [JsonProperty("provisioningState")]
        public string ProvisioningState { get; set; }
    }
}
