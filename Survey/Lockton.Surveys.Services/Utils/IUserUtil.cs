using System.Security.Claims;

namespace Lockton.Surveys.Services.Utils
{
    public interface IUserUtil
    {
        int GetUserID(ClaimsPrincipal user);
        string GetUserName(ClaimsPrincipal user);
        string GetName(ClaimsPrincipal user);
    }
}