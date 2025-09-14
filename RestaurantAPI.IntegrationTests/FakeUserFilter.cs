using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestaurantAPI.IntegrationTests;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrinciple = new ClaimsPrincipal();

        claimsPrinciple.AddIdentity(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "Admin"),

        ]));

        context.HttpContext.User = claimsPrinciple;

        await next();
    }
}