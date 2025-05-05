using Lockton.Surveys.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Web.Configuration
{
    public static class AuthenticationConfigurationExtension
    {

        public static void addAuthenticationService(this IServiceCollection service, IConfiguration configuration)
        {
            IConfigurationSection bearerConfig = configuration.GetSection("Authentication:Bearer");
            string secretKey = bearerConfig["Secret"];

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters =
                      new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidateAudience = true,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          ValidIssuer = bearerConfig["Issuer"],
                          ValidAudience = bearerConfig["Audience"],
                          IssuerSigningKey = key
                      };
             });

            service.AddAuthorization(options =>
            {
                //options.AddPolicy("ReadAccess", policy =>
                //{
                //    policy.Requirements.Add(new SeguridadServiceRequirement("R"));
                //    policy.RequireAssertion(D => D.User.Claims.Where(C => C.Type == ClaimTypes.AuthorizationDecision).Count() > 0);

                //});
                //options.AddPolicy("WriteAccess", policy =>
                //{
                //    policy.Requirements.Add(new SeguridadServiceRequirement("W"));
                //    policy.RequireAssertion(D => D.User.Claims.Where(C => C.Type == ClaimTypes.AuthorizationDecision).Count() > 0);
                //});
                //options.AddPolicy("DeleteAccess", policy =>
                //{
                //    policy.Requirements.Add(new SeguridadServiceRequirement("D"));
                //    policy.RequireAssertion(D => D.User.Claims.Where(C => C.Type == ClaimTypes.AuthorizationDecision).Count() > 0);
                //});
            });

            service.AddTransient<IAuthorizationHandler, SeguridadServiceHandler>();
            service.AddSingleton<IAuthorizationPolicyProvider, SeguridadServiceProvider>();

        }



    }
}
