using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Entities.Validators;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Mapping;

namespace RestaurantAPI.Services;

public interface IAccountService
{
    void RegisterUser(RegisterUserDto dto);
    string GenerateJwt(LoginUserDto dto);
}

public class AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings) : IAccountService
{
    public void RegisterUser(RegisterUserDto dto)
    {
        var newUser = dto.ToUser(passwordHasher);
        dbContext.Users.Add(newUser);
        dbContext.SaveChanges();
    }

    public string GenerateJwt(LoginUserDto dto)
    {
        var user = dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Email == dto.Email);
        if (user is null)
        {
            throw new BadRequestException("Invalid username or password");
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new BadRequestException("Invalid username or password");
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(
            authenticationSettings.JwtIssuer,
            authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

}