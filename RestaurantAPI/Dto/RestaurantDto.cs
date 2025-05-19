namespace RestaurantAPI.Dto;

public record RestaurantDto(
    int Id,
    string Name,
    string Description,
    string Category,
    bool HasDelivery,
    string City,
    string Street,
    string PostalCode,
    List<DishDto> Dishes
);