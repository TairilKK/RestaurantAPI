using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorizations;

public class CreatedMultipleRestaurantsRequirement(int MinimumRestaurantsCreated) : IAuthorizationRequirement
{
    public int MinimumRestaurantsCreated;
}