using RestaurantAPI.Dto;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Mapping;

public static class DishMapping
{
    public static DishDto ToDishDto(this Dish dish)
    {
        return new DishDto(
            dish.Id,
            dish.Name,
            dish.Description,
            dish.Price
        );
    }
}