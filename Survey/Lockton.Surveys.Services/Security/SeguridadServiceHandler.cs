
using Lockton.Surveys.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Lockton.Surveys.DataAccess.Repositories;
using Lockton.Surveys.DataAccess.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Lockton.Surveys.Services.Security
{
    public class SeguridadServiceHandler : AuthorizationHandler<SeguridadServiceRequirement>
    {
        private IUserUtil _userUtil;
        private readonly IRepository<User> _repositoryUser;
        public SeguridadServiceHandler(IUserUtil userUtil, IRepository<User> repositoryUser) : base()
        {
            _userUtil = userUtil;
            _repositoryUser = repositoryUser;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SeguridadServiceRequirement requirement)
        {
            try
            {
                var Screen = (context.Resource as Microsoft.AspNetCore.Http.HttpContext).GetRouteValue("controller");
                if (!string.IsNullOrEmpty(requirement.Module))
                {
                    Screen = requirement.Module;
                }

                int user = _userUtil.GetUserID(context.User);
                var dbUser = _repositoryUser.GetByCondition(D => D.Id == user)
                    .Include(p => p.Profile)
                    .Include(um => um.Modules)
                    .ThenInclude(m => m.Module).FirstOrDefault();
                
                var dbProfile = dbUser.Profile;
                foreach (var module in dbUser.Modules)
                {

                    if (module.Module.Name == (string)Screen)
                    {
                        string currentAcces = module.Privilege;

                        if (currentAcces == "ALL")
                        {
                            currentAcces = "RWD";
                        }
                        if (currentAcces.Contains(requirement.Rigth))
                        {
                            if (string.IsNullOrEmpty(requirement.Profile))
                            {
                                context.Succeed(requirement);
                                return;
                            }
                            else
                            {
                                if (requirement.Profile == dbUser.Profile.Name)
                                {
                                    context.Succeed(requirement);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return;
        }
    }
}
