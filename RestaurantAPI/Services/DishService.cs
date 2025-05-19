using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Mapping;

namespace RestaurantAPI.Services;

public interface IDishService
{
    int CreateDish(int restaurantId, CreateDishDto dto);
    Dish GetById(int restaurantId, int dishId);
    IEnumerable<DishDto> GetAll(int restaurantId);
    void DeleteDish(int restaurantId, int dishId);
    void DeleteAllDishes(int restaurantId);
}

public class DishService (RestaurantDbContext dbContext) : IDishService
{
    public void DeleteDish(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurantWithDishes(restaurantId);

        var dish = restaurant.Dishes
            .FirstOrDefault(d => d.RestaurantId == restaurantId && d.Id == dishId);

        if (dish is null)
            throw new NotFoundException("Dish not found");

        dbContext.Dishes.Remove(dish);
        dbContext.SaveChanges();
    }

    public void DeleteAllDishes(int restaurantId)
    {
        var restaurant = GetRestaurantWithDishes(restaurantId);

        var dish = restaurant.Dishes.ToList();

        if (dish is null)
            throw new NotFoundException("Dish not found");

        dbContext.Dishes.RemoveRange(dish);
        dbContext.SaveChanges();
    }

    public int CreateDish(int restaurantId, CreateDishDto dto)
    {
        var restaurant = dbContext.Restaurants
            .FirstOrDefault(r => r.Id == restaurantId);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var newDish = dto.ToDish();
        newDish.RestaurantId = restaurantId;

        dbContext.Dishes.Add(newDish);
        dbContext.SaveChanges();

        return newDish.Id;
    }

    public Dish GetById(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurantWithDishes(restaurantId);

        var dish = restaurant.Dishes
            .FirstOrDefault(d => d.RestaurantId == restaurantId && d.Id == dishId);

        if (dish is null)
            throw new NotFoundException("Dish not found");

        return dish;
    }

    public IEnumerable<DishDto> GetAll(int restaurantId)
    {
        var restaurant = GetRestaurantWithDishes(restaurantId);

        var dishes = restaurant.Dishes
            .Select(d => d.ToDishDto());

        return dishes;
    }

    private Restaurant? GetRestaurantWithDishes(int restaurantId)
    {
        var restaurant = dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == restaurantId);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        return restaurant;
    }
}