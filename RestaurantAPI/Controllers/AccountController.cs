using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController(RestaurantDbContext dbContext, IAccountService accountService) : ControllerBase
{
    [HttpPost("register")]
    public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
    {
        accountService.RegisterUser(dto);
        return Ok();
    }

    [HttpPost("login")]
    public ActionResult LoginUser([FromBody] LoginUserDto dto)
    {
        var token = accountService.GenerateJwt(dto);
        return Ok(token);
    }
}