using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silica_Animus.Builders;
using Silica_Animus.Repository;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Silica_Animus.Model
{
    public class Register
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Pass. length should be between 6 and 20", MinimumLength = 6)]
        public string Password { get; set; }

        public async Task<IActionResult> RegisterUser(IAuthRepository authRepository, Register registerModel)
        {
            if (authRepository.UserManager.Users.Any(x => x.UserName == registerModel.Email))
            {
                return authRepository.BaseResultBuilder.ClientExistResult();
            }

            var user = new IdentityUser
            {
                UserName = registerModel.Username,
                Email = registerModel.Email
            };

            IActionResult actionResult = await authRepository.UserManager.CreateAsync(user, registerModel.Password)
                                                          .ContinueWith((result) =>
                                                          {
                                                              return result.Result.Succeeded
                                                             ? authRepository.BaseResultBuilder.BuildOkResult(authRepository.TokenBuilder.GenerateJwtToken(user.Email, user, authRepository.IdConf))
                                                             : (IActionResult)authRepository.BaseResultBuilder.RegisterRuleViolation(result.Result.Errors.SingleOrDefault().Description);
                                                          });
            if (actionResult is OkObjectResult)
            {
                await authRepository.SignInManager.SignInAsync(user, false);
            }
            return actionResult;
        }
    }
}
