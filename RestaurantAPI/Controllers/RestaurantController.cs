using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
public class RestaurantController(RestaurantDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Restaurant>> GetAll()
    {
        var restaurants = dbContext.Restaurants.ToList();
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    public ActionResult<IEnumerable<Restaurant>> Get([FromRoute] int id)
    {
        var restaurant = dbContext.Restaurants
            .FirstOrDefault(r => r.Id == id);

        return restaurant is not null
            ? Ok(restaurant)
            : NotFound();
    }
}