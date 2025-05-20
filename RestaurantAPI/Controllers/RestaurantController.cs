using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Mapping;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
[ApiController]
public class RestaurantController(IRestaurantService restaurantService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        return Ok(restaurantService.GetAll());
    }

    [HttpGet("{id:int}")]
    public ActionResult<IEnumerable<RestaurantDto>> Get([FromRoute] int id)
    {
        var restaurant = restaurantService.GetById(id);

        return Ok(restaurant);
    }

    [HttpPost]
    public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        var id = restaurantService.Create(dto);

        return Created($"/api/restaurants/{id}", null);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteRestaurant([FromRoute] int id)
    {
        restaurantService.Delete(id);

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
    {
        restaurantService.Update(id, dto);

        return Ok(id);
    }
}