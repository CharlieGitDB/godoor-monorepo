
using AutoWrapper.Wrappers;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Web;

namespace Identity.API.Filters;

//not used currently
public class MeFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = context.HttpContext.User.GetObjectId();

        var routeUserId = context.ActionArguments["userId"] as string;

        if (routeUserId == null || routeUserId != userId)
        {
            throw new ApiProblemDetailsException("Unable to access content", StatusCodes.Status404NotFound);
        }

        await next();
    }
}