using System.Security.Claims;

namespace RestaurantAPI.Services;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    int? GetUserId { get; }
}

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    public ClaimsPrincipal User => httpContextAccessor.HttpContext?.User;

    public int? GetUserId =>
        int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

}