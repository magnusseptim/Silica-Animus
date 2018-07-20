using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Silica_Animus.Builders;
using Silica_Animus.Helpers;
using Silica_Animus.Model;
using Silica_Animus.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace Silica_Animus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAuthRepository authRepository;
        //private readonly SignInManager<IdentityUser> signInManager;
        //private readonly UserManager<IdentityUser> userManager;
        //private readonly IConfiguration appConfiguration;
        //private readonly IdentityConf idConf;
        //private readonly ITokenBuilder tokenBuilder;
        //private readonly IBaseResultBuilder baseResultBuilder;

        public AccountController
        (
            IAuthRepository authRepository
            //SignInManager<IdentityUser> signInManager,
            //UserManager<IdentityUser> userManager,
            //IConfiguration appConfiguration,
            //ITokenBuilder tokenBuilder,
            //IBaseResultBuilder baseResultBuilder
        )
        {
            this.authRepository = authRepository;
            //this.signInManager = signInManager;
            //this.userManager = userManager;
            //this.appConfiguration = appConfiguration;
            //this.tokenBuilder = tokenBuilder;
            //this.baseResultBuilder = baseResultBuilder;
            // Read connection string from secure place
            // Here simple example, VerySafePlace should be in fact safe!
            //this.idConf = FileReader.ReadAsJSONFromFile<IdentityConf>(this.appConfiguration["ConfigurationPath"]);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LogIn loginModel)
        {
            return await loginModel.SignInUser(authRepository,loginModel);
            //var user = userManager.Users.Where(x => x.UserName == loginModel.Username).SingleOrDefault();
            //return await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)
            //                    .ContinueWith((result) =>
            //                    {
            //                        return result.Result.Succeeded
            //                        ? new OkObjectResult(tokenBuilder.GenerateJwtToken(
            //                                                                            loginModel.Email, 
            //                                                                            userManager.Users
            //                                                                                       .SingleOrDefault(x => x.Email == loginModel.Email),
            //                                                                            idConf
            //                                                                           ))
            //                        : (IActionResult)baseResultBuilder.BadCredentialsResult();
            //                    });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Register registerModel)
        {
            return await registerModel.RegisterUser(authRepository,registerModel);
           //if (userManager.Users.Any(x => x.UserName == registerModel.Email))
           //{
           //     return baseResultBuilder.ClientExistResult();
           //}

           //var user = new IdentityUser
           //{
           //     UserName = registerModel.Username,
           //     Email = registerModel.Email
           //};

           //IActionResult actionResult = await userManager.CreateAsync(user,registerModel.Password)
           //                                              .ContinueWith((result) =>
           //                                              {
           //                                                 return result.Result.Succeeded
           //                                                 ? baseResultBuilder.BuildOkResult(tokenBuilder.GenerateJwtToken(user.Email, user,idConf))
           //                                                 : (IActionResult)baseResultBuilder.RegisterRuleViolation(result.Result.Errors.SingleOrDefault().Description);
           //                                              });
           // if (actionResult is OkObjectResult)
           // {
           //     await signInManager.SignInAsync(user, false);
           // }
           // return actionResult;        
        }
    }
}
