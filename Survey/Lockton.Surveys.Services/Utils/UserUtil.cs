using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Lockton.Surveys.Services.Utils
{
    public class UserUtil : IUserUtil
    {
        public int GetUserID(ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.Where(c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);
        }

        public string GetUserName(ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
        }

        public string GetName(ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
        }
    }
}
