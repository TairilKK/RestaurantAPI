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

    public static Restaurant ToRestaurant(this CreateRestaurantDto createRestaurantDto)
    {
        return new Restaurant()
        {
            Name = createRestaurantDto.Name,
            Description = createRestaurantDto.Description,
            Category = createRestaurantDto.Category,
            HasDelivery = createRestaurantDto.HasDelivery,
            ContactEmail = createRestaurantDto.ContactEmail,
            ContactNumber = createRestaurantDto.ContactNumber,
            Adress = new Adress()
            {
                City = createRestaurantDto.City,
                Street = createRestaurantDto.Street,
                PostalCode = createRestaurantDto.PostalCode
            }
        };
    }
}