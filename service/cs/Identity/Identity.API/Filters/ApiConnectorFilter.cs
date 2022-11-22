using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.API.Filters;

public class ApiConnectorAttirbute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        
    }
}