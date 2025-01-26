using habytee.Interconnection.Models;
using habytee.Server.DataAccess;
using habytee.Server.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace habytee.Server.Core;

[ServiceFilter(typeof(UserAuthenticationFilter))]
public abstract class BaseController : ControllerBase
{
    protected User CurrentUser => (User)HttpContext.Items["CurrentUser"]!;
    protected Habit? CurrentHabit => (Habit?)HttpContext.Items["CurrentHabit"];
    protected WriteDbContext WriteDbContext => (WriteDbContext)HttpContext.RequestServices.GetService(typeof(WriteDbContext))!;
    protected ReadDbContext ReadDbContext => (ReadDbContext)HttpContext.RequestServices.GetService(typeof(ReadDbContext))!;
    protected GetUserService DataService => (GetUserService)HttpContext.RequestServices.GetService(typeof(GetUserService))!;
}
