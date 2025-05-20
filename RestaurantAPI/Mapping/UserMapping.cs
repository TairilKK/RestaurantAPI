using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Mapping;

public static class UserMapping
{
    public static User ToUser(this RegisterUserDto dto, IPasswordHasher<User> passwordHasher)
    {
        var newUser = new User()
        {
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            Nationality = dto.Nationality,
            RoleId = dto.RoleId
        };
        newUser.PasswordHash = passwordHasher.HashPassword(newUser, dto.Password);

        return newUser;
    }
}