using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Silica_Animus.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Silica_Animus.Builders
{
    public class TokenBuilder : ITokenBuilder
    {
        public string GenerateJwtToken(string email, IdentityUser user, IdentityConf idConf)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId,user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(idConf.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // TODO : Check if UtcNow will handle correct
            var expires = DateTime.UtcNow.AddDays(Convert.ToInt32(idConf.JwtExpireDays));

            SecurityToken token = new JwtSecurityToken(idConf.JwtIssuer,
                                                        idConf.JwtIssuer,
                                                        claims,
                                                        expires: expires,
                                                        signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
