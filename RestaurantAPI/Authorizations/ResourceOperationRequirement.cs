using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorizations;

public enum ResourceOperation
{
    Create,
    Read,
    Update,
    Delete
}

public class ResourceOperationRequirement(ResourceOperation resourceOperation) : IAuthorizationRequirement
{
    public ResourceOperation ResourceOperation { get; } = resourceOperation;
}