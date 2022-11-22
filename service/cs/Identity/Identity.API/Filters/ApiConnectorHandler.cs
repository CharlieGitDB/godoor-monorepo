using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Filters;

public class ApiConnectorHandler : AuthorizationHandler<ApiConnectorRequirement>
{
    IHttpContextAccessor? _httpContextAccessor = null;

    public ApiConnectorHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiConnectorRequirement requirement)
    {
        var reqHeaders = _httpContextAccessor?.HttpContext?.Request.Headers;

        if (!reqHeaders.ContainsKey("Authorization"))
        {
            context.Fail();
            return Task.FromResult(false);
        }

        var auth = reqHeaders["Authorization"].ToString();

        if (!auth.StartsWith("Basic "))
        {
            context.Fail();
            return Task.FromResult(false);
        }

        var cred = System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');

        if (cred[0] == requirement.Username && cred[1] == requirement.Password)
        {
            context.Succeed(requirement);
            return Task.FromResult(true);
        }
        else
        {
            context.Fail();
            return Task.FromResult(false);
        }
    }


}