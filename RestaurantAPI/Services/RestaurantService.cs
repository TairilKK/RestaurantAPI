using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorizations;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Mapping;

namespace RestaurantAPI.Services;

public interface IRestaurantService
{
    RestaurantDto? GetById(int id);
    IEnumerable<RestaurantDto> GetAll();
    int Create(CreateRestaurantDto dto);
    void Delete(int id);
    void Update(int id, UpdateRestaurantDto dto);
}

public class RestaurantService(RestaurantDbContext dbContext, ILogger<RestaurantService> logger, IAuthorizationService authorizationService, IUserContextService userContextService) : IRestaurantService
{
    public RestaurantDto? GetById(int id)
    {
        var restaurant = dbContext.Restaurants
            .Include(r => r.Adress)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        return restaurant.ToRestaurantDto();
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
        newRestaurant.CreatedById = userContextService.GetUserId;
        dbContext.Restaurants.Add(newRestaurant);
        dbContext.SaveChanges();

        return newRestaurant.Id;

    }

    public void Delete(int id)
    {
        logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");

        var restaurant = dbContext.Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, restaurant,
            new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException();
        }

        dbContext.Restaurants.Remove(restaurant);
        dbContext.SaveChanges();
    }

    public void Update(int id, UpdateRestaurantDto dto)
    {
        var restaurant = dbContext.Restaurants
            .FirstOrDefault(r => r.Id == id);
        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, restaurant,
            new ResourceOperationRequirement(ResourceOperation.Update)).Result;

        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException();
        }
        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.HasDelivery = dto.HasDelivery;

        dbContext.SaveChanges();
    }
}