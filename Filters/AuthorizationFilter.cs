using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microservice.Filters
{
    public class ApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private readonly string _expectedApiKey = "secretkey"; // Substitua pela sua chave de API

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var apiKey = context.HttpContext.Request.Headers["ApiKey"].ToString();

            if (apiKey != _expectedApiKey)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
