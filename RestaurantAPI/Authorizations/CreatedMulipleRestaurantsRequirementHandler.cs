using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorizations;

public class CreatedMultipleRestaurantsRequirementHandler(RestaurantDbContext dbContext) : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
    {
        var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

        var createdRestaurantsCount = dbContext.Restaurants.Count(r => r.CreatedById == userId);

        if (createdRestaurantsCount > requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}