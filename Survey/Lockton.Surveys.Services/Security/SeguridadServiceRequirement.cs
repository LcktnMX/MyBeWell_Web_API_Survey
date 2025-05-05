using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lockton.Surveys.Services.Security
{
    public class SeguridadServiceRequirement : IAuthorizationRequirement
    {
        public string Rigth { get; }

        public string Module { get; }
        public string Profile { get; }
        public SeguridadServiceRequirement(string rigth, string module)
        {
            Rigth = rigth;
            Module = module;
        }

        public SeguridadServiceRequirement(string rigth, string module, string profile)
        {
            Rigth = rigth;
            Module = module;
            Profile = profile;
        }
    }
}
