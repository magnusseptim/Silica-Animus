using Microsoft.AspNetCore.Mvc;
using Silica_Animus.Model;
using Silica_Animus.Repository;
using System.Threading.Tasks;

namespace Silica_Animus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAuthRepository authRepository;
        public AccountController( IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LogIn loginModel)
        {
            return await loginModel.SignInUser(authRepository,loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Register registerModel)
        {
           return await registerModel.RegisterUser(authRepository,registerModel);
        }
    }
}
