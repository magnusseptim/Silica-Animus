using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Silica_Animus.Builders;
using Silica_Animus.Model;

namespace Silica_Animus.Repository
{
    public interface IAuthRepository
    {
        IConfiguration AppConfiguration { get; }
        IBaseResultBuilder BaseResultBuilder { get; }
        IdentityConf IdConf { get; }
        SignInManager<IdentityUser> SignInManager { get; }
        ITokenBuilder TokenBuilder { get; }
        UserManager<IdentityUser> UserManager { get; }
    }
}