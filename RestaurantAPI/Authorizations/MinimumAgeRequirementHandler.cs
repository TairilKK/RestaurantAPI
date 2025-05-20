using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorizations;

public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
        var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
        logger.LogInformation($"User: {userEmail} with date of birth: [{dateOfBirth}]");
        if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
        {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            logger.LogInformation("Authorization failed");
        }

        return Task.CompletedTask;
    }
}