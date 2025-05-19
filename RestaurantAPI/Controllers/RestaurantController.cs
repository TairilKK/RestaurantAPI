using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Mapping;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
public class RestaurantController(IRestaurantService restaurantService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        return Ok(restaurantService.GetAll());
    }

    [HttpGet("{id:int}")]
    public ActionResult<IEnumerable<RestaurantDto>> Get([FromRoute] int id)
    {
        var restaurant = restaurantService.GetById(id);

        if (restaurant is null)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpPost]
    public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var id = restaurantService.Create(dto);

        return Created($"/api/restaurants/{id}", null);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteRestaurant([FromRoute] int id)
    {
        var isDeleted = restaurantService.Delete(id);

        return isDeleted
            ? NoContent()
            : NotFound();
    }
}