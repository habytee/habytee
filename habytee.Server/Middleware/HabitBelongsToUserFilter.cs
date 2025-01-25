using habytee.Interconnection.Models;
using habytee.Server.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace habytee.Server.Middleware;

public class HabitBelongsToUserFilter : IAsyncActionFilter
{
    private readonly ReadDbContext readDbContext;

    public HabitBelongsToUserFilter(ReadDbContext readDbContext)
    {
        this.readDbContext = readDbContext;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var habitId = context.ActionArguments.Values
            .FirstOrDefault(v => v is int id && context.ActionDescriptor.Parameters
                .Any(p => p.Name == "id" && p.ParameterType == typeof(int))) as int?;

        if (habitId.HasValue)
        {
            var currentUser = (User)context.HttpContext.Items["CurrentUser"]!;
            var habit = readDbContext.Habits
                .Include(h => h.HabitCheckedEvents)
                .FirstOrDefault(h => h.Id == habitId && h.UserId == currentUser.Id);

            if (habit == null)
            {
                context.Result = new NotFoundObjectResult("Habit not found or access denied.");
                return;
            }

            context.HttpContext.Items["CurrentHabit"] = habit;
        }

        await next();
    }
}
