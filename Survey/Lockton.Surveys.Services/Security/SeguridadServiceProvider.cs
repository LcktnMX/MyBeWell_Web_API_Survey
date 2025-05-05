
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Services.Security
{
    public class SeguridadServiceProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "SecurityAuto";

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
           return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {

                string[] PolicyAccess = policyName.Split('_');

                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
                policy.Requirements.Add(new SeguridadServiceRequirement(PolicyAccess[1], PolicyAccess[2], PolicyAccess[3]));
                policy.RequireAssertion(D => D.User.Claims.Where(C => C.Type == ClaimTypes.AuthorizationDecision).Count() > 0);
               
                return Task.FromResult(policy.Build());
            }

            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}
