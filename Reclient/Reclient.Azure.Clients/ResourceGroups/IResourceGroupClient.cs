using Reclient.Azure.Clients.Models;
using Refit;
using System.Threading.Tasks;

namespace Reclient.Azure.Clients.ResourceGroups
{

    public interface IResourceGroupClient
    {
        [Headers("Authorization: Bearer")]
        [Put("/subscriptions/{subscriptionId}/resourcegroups/{resourceGroupName}?api-version=2019-05-10")]
        Task<ResourceGroup> CreateResourceGroupAsync([AliasAs("subscriptionId")] string subscriptionId, [AliasAs("resourceGroupName")] string resourceGroupName, [Body] ResourceGroupRequest resourceGroupRequest);
    }
}
