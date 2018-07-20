using Microsoft.AspNetCore.Mvc;

namespace Silica_Animus.Builders
{
    public interface IBaseResultBuilder
    {
        OkObjectResult BuildOkResult(object obj);
        BadRequestObjectResult ClientExistResult();
        BadRequestObjectResult BadCredentialsResult();
        BadRequestObjectResult RegisterRuleViolation(string message);
    }
}