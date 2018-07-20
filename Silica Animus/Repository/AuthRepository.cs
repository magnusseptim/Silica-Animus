using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Silica_Animus.Builders;
using Silica_Animus.Helpers;
using Silica_Animus.Model;

namespace Silica_Animus.Repository
{
    public class AuthRepository : IAuthRepository
    {
        public SignInManager<IdentityUser> SignInManager { get; }
        public UserManager<IdentityUser> UserManager { get; }
        public IConfiguration AppConfiguration { get; }
        public IdentityConf IdConf { get; }
        public ITokenBuilder TokenBuilder { get; }
        public IBaseResultBuilder BaseResultBuilder { get; }

        public AuthRepository
        (
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IConfiguration appConfiguration,
            ITokenBuilder tokenBuilder,
            IBaseResultBuilder baseResultBuilder
        )
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            this.AppConfiguration = appConfiguration;
            this.TokenBuilder = tokenBuilder;
            this.BaseResultBuilder = baseResultBuilder;
            // Read connection string from secure place
            // Here simple example, VerySafePlace should be in fact safe!
            this.IdConf = FileReader.ReadAsJSONFromFile<IdentityConf>(this.AppConfiguration["ConfigurationPath"]);
        }
    }
}
