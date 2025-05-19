using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Mapping;

namespace RestaurantAPI.Services;

public interface IRestaurantService
{
    RestaurantDto? GetById(int id);
    IEnumerable<RestaurantDto> GetAll();
    int Create(CreateRestaurantDto dto);
    bool Delete(int id);
}

public class RestaurantService(RestaurantDbContext dbContext) : IRestaurantService
{
    public RestaurantDto? GetById(int id)
    {
        var restaurant = dbContext.Restaurants
            .Include(r => r.Adress)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id);

        return restaurant?.ToRestaurantDto();
    }

    public IEnumerable<RestaurantDto> GetAll()
    {
        var restaurants = dbContext.Restaurants
            .Include(r => r.Dishes)
            .Include(r => r.Adress)
            .ToList();

        var restaurantsDtos = restaurants
            .Select(r => r.ToRestaurantDto())
            .ToList();

        return restaurantsDtos;
    }

    public int Create(CreateRestaurantDto dto)
    {
        var newRestaurant = dto.ToRestaurant();
        dbContext.Restaurants.Add(newRestaurant);
        dbContext.SaveChanges();
        return newRestaurant.Id;
    }

    public bool Delete(int id)
    {
        var restaurant = dbContext.Restaurants
            .FirstOrDefault(r => r.Id == id);
        if (restaurant is null) return false;

        dbContext.Restaurants.Remove(restaurant);
        dbContext.SaveChanges();
        return true;
    }
}