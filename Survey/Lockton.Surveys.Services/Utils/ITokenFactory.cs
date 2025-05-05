using System.Security.Claims;
using System.Threading.Tasks;

namespace Lockton.Surveys.Services.Utils
{
    public interface ITokenFactory
    {
        string GenerateToken(string user, ClaimsIdentity identity, int ExpirationHours=24);
        bool ValidateJwtToken(string token);
    }
}