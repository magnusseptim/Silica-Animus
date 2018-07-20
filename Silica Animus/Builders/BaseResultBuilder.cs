using Microsoft.AspNetCore.Mvc;

namespace Silica_Animus.Builders
{
    public class BaseResultBuilder : IBaseResultBuilder
    {
        public OkObjectResult BuildOkResult(object obj)
        {
            return new OkObjectResult(obj);
        }
        public BadRequestObjectResult BadCredentialsResult()
        {
            return new BadRequestObjectResult("Incorrect email or password");
        }
        public BadRequestObjectResult ClientExistResult()
        {
            return new BadRequestObjectResult("Client exist");
        }
        public BadRequestObjectResult RegisterRuleViolation(string message)
        {
            return new BadRequestObjectResult(message);
        }
    }
}
