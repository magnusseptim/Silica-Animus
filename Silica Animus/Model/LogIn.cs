using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silica_Animus.Builders;
using Silica_Animus.Repository;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Silica_Animus.Model
{
    public class LogIn
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public async Task<IActionResult> SignInUser(IAuthRepository authRepository ,LogIn loginModel)
        {
            var user = authRepository.UserManager.Users.Where(x => x.UserName == loginModel.Username).SingleOrDefault();
            return await authRepository.SignInManager.PasswordSignInAsync(user, loginModel.Password, false, false)
                                .ContinueWith((result) =>
                                {
                                    return result.Result.Succeeded
                                    ? new OkObjectResult(authRepository.TokenBuilder.GenerateJwtToken(
                                                                                        loginModel.Email,
                                                                                        authRepository.UserManager.Users
                                                                                                   .FirstOrDefault(x => x.Email == loginModel.Email),
                                                                                         authRepository.IdConf
                                                                                       ))
                                    : (IActionResult)authRepository.BaseResultBuilder.BadCredentialsResult();
                                });
        }
    }
}
