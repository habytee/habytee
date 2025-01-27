namespace habytee.Server.Middleware;

using habytee.Server.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class UserAuthenticationFilter : IAsyncActionFilter
{
    private readonly GetUserService getUserService;

    public UserAuthenticationFilter(GetUserService getUserService)
    {
        this.getUserService = getUserService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var email = context.HttpContext.Request.Headers["X-Forwarded-Email"].FirstOrDefault();
        if (email == null)
        {
            context.Result = new BadRequestObjectResult("OAuth2 proxy error");
            return;
        }

        var user = getUserService.GetReadUser(email);
        context.HttpContext.Items["CurrentUser"] = user;

        await next();
    }
}
