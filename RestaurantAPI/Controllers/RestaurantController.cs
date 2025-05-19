using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Mapping;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
public class RestaurantController(RestaurantDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        var restaurants = dbContext.Restaurants
            .Include(r => r.Dishes)
            .Include(r => r.Adress)
            .ToList();

        var restaurantsDtos = restaurants
            .Select(r => r.ToRestaurantDto())
            .ToList();

        return Ok(restaurantsDtos);
    }

    [HttpGet("{id:int}")]
    public ActionResult<IEnumerable<RestaurantDto>> Get([FromRoute] int id)
    {
        var restaurant = dbContext.Restaurants
            .Include(r => r.Adress)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
        {
            return NotFound();
        }

        return Ok(restaurant.ToRestaurantDto());
    }
}