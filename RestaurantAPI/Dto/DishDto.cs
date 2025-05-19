namespace RestaurantAPI.Dto;

public record DishDto(
    int Id,
    string Name,
    string? Description,
    decimal Price
);