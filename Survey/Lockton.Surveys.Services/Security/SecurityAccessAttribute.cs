using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lockton.Surveys.Services.Security
{
    public class SecurityAccessAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "SecurityAuto";

        public SecurityAccessAttribute(string Rigth, string Module,string Profile)
        {
            Policy = $"{POLICY_PREFIX}_{Rigth}_{Module}_{Profile}";
        }

        public SecurityAccessAttribute(string Rigth, string Module)
        {
            Policy = $"{POLICY_PREFIX}_{Rigth}_{Module}_";
        }

        public SecurityAccessAttribute(string Rigth)
        {
            Policy = $"{POLICY_PREFIX}_{Rigth}_";
        }

    }
}
