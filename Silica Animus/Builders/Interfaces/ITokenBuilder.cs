using Microsoft.AspNetCore.Identity;
using Silica_Animus.Model;

namespace Silica_Animus.Builders
{
    public interface ITokenBuilder
    {
        string GenerateJwtToken(string email, IdentityUser user, IdentityConf idConf);
    }
}