using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant/{restaurantId:int}/dish")]
[ApiController]
public class DishController(IDishService dishService) : ControllerBase
{
    [HttpDelete]
    public ActionResult RemoveAllDishes([FromRoute] int restaurantId)
    {
        dishService.DeleteAllDishes(restaurantId);
        return NoContent();
    }

    [HttpDelete("{dishId:int}")]
    public ActionResult RemoveById([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        dishService.DeleteDish(restaurantId, dishId);
        return NoContent();
    }

    [HttpGet]
    public ActionResult<IEnumerable<DishDto>> GetAll([FromRoute] int restaurantId)
    {
        var dish = dishService.GetAll(restaurantId);
        return Ok(dish);
    }
    [HttpGet("{dishId:int}")]
    public ActionResult<Dish> GetById([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = dishService.GetById(restaurantId, dishId);
        return Ok(dish);
    }
    [HttpPost]
    public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
    {
        var dishId = dishService.CreateDish(restaurantId, dto);
        return Created($"api/restaurant/{restaurantId}/dish/{dishId}", null);
    }
}