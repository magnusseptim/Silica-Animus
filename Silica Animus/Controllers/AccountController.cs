using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Silica_Animus.Helpers;
using Silica_Animus.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Silica_Animus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration appConfiguration;
        private readonly IdentityConf idConf;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager ,IConfiguration appConfiguration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.appConfiguration = appConfiguration;
            // Read connection string from secure place
            // Here simple example, VerySafePlace should be in fact safe!
            this.idConf = FileReader.ReadAsJSONFromFile<IdentityConf>(this.appConfiguration["ConfigurationPath"]);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LogIn loginModel)
        {
            return await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false)
                                .ContinueWith((result) =>
                                {
                                    return result.Result.Succeeded
                                    ? new OkObjectResult(GenerateJwtToken(loginModel.Email, userManager.Users.SingleOrDefault(x => x.Email == loginModel.Email)))
                                    : (IActionResult)new BadRequestObjectResult("Incorrect email or password");
                                });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Register registerModel)
        {
           if (userManager.Users.Any(x => x.UserName == registerModel.Email))
           { 
                return new BadRequestObjectResult("Client exist");
           }

           var user = new IdentityUser
           {
                UserName = registerModel.Email,
                Email = registerModel.Email
           };

           var jwt = await userManager.CreateAsync(user)
                                      .ContinueWith((result) =>
                                       {
                                          return result.IsCompletedSuccessfully
                                                 ? GenerateJwtToken(user.Email, user)
                                                 : string.Empty;
                                       });
            await signInManager.SignInAsync(user, false);
            return new OkObjectResult(jwt);                   
        }

        private string GenerateJwtToken(string email, IdentityUser user)
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

            SecurityToken token = new JwtSecurityToken( idConf.JwtIssuer,
                                                        idConf.JwtIssuer,
                                                        claims,
                                                        expires: expires,
                                                        signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
