using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Services.Utils
{
    public class TokenGenerator : ITokenFactory
    {
        IConfiguration _configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string user, ClaimsIdentity identity,int ExpirationMinutes=60)
        {
            var secretKey = _configuration.GetValue<string>("Authentication:Bearer:Secret");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var Audience = _configuration.GetValue<string>("Authentication:Bearer:Audience");
            var Issuer = _configuration.GetValue<string>("Authentication:Bearer:Issuer");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Audience = Audience,
                Issuer = Issuer,
                Expires = DateTime.UtcNow.AddMinutes(ExpirationMinutes),                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);


        }

        public bool ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = _configuration.GetValue<string>("Authentication:Bearer:Secret");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var Audience = _configuration.GetValue<string>("Authentication:Bearer:Audience");
            var Issuer = _configuration.GetValue<string>("Authentication:Bearer:Issuer");
            SymmetricSecurityKey keyID = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = keyID,
                   ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                return true;
            }
            catch
            {
                // return null if validation fails
                return false;
            }
        }
    }
}
