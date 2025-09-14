using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.Entities.Validators;

namespace RestaurantAPI.IntegrationTests.Validators;

public class RestaurantUserDtoValidatorTests
{
    private readonly RestaurantDbContext _dbContext;

    public RestaurantUserDtoValidatorTests()
    {
        var builder = new DbContextOptionsBuilder<RestaurantDbContext>();
        builder.UseInMemoryDatabase("TestDb");

        _dbContext = new RestaurantDbContext(builder.Options);
        Seed();
    }

    private void Seed()
    {
        var testUsers = new List<User>()
        {
            new ()
            {
                Email = "test2@test.com",
                PasswordHash = "a",
            },
            new ()
            {
                Email = "test3@test.com",
                PasswordHash = "a",
            }
        };

        _dbContext.Users.AddRange(testUsers);
        _dbContext.SaveChanges();
    }

    [Fact]
    public void Validate_ForValidModel_ReturnsSuccess()
    {
        var model = new RegisterUserDto(
            "test@test.com",
            "123456",
            "123456",
            null,
            null);

        var validator = new RegisterUserDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact]
    public void Validate_ForInvalidModel_ReturnsFail()
    {
        var model = new RegisterUserDto(
            "test2@test.com",
            "123456",
            "123456",
            null,
            null);

        var validator = new RegisterUserDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }

}