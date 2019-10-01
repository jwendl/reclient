using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reclient.Authentication.Interfaces
{
    public interface ITokenCreator
    {
        Task<string> GetAccessTokenAsync(IEnumerable<string> scopes);
    }
}
