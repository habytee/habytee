namespace habytee.Server.Middleware;

using habytee.Server.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class UserAuthenticationFilter : IAsyncActionFilter
{
    private readonly DataService dataService;

    public UserAuthenticationFilter(DataService dataService)
    {
        this.dataService = dataService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var email = context.HttpContext.Request.Headers["X-Forwarded-Email"].FirstOrDefault();
        if (email == null)
        {
            context.Result = new BadRequestObjectResult("OAuth2 proxy error");
            return;
        }

        var user = dataService.GetReadUser(email);
        context.HttpContext.Items["CurrentUser"] = user;

        await next();
    }
}
