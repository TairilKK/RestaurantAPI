using RestaurantAPI.Dto;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Mapping;

public static class RestaurantMapping
{
    public static RestaurantDto ToRestaurantDto(this Restaurant restaurant)
    {
        return new RestaurantDto(
            Id: restaurant.Id,
            Name: restaurant.Name,
            Description: restaurant.Description,
            Category: restaurant.Category,
            HasDelivery: restaurant.HasDelivery,
            City: restaurant.Adress.City,
            Street: restaurant.Adress.Street,
            PostalCode: restaurant.Adress.PostalCode,
            Dishes: restaurant.Dishes.Select(d => d.ToDishDto()).ToList()
        );
    }
}