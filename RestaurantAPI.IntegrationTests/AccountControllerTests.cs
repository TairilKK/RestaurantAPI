using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Services;

namespace RestaurantAPI.IntegrationTests;

public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private Mock<IAccountService> accountServiceMock = new Mock<IAccountService>();

    public AccountControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));
                    services.Remove(dbContextOptions);

                    services.AddSingleton(accountServiceMock.Object);
                    services.AddDbContext<RestaurantDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("RestaurantDb");
                    });
                });
            });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task LoginUser_ForRegisteredUser_ReturnsOk()
    {
        accountServiceMock
            .Setup(e => e.GenerateJwt(It.IsAny<LoginUserDto>()))
            .Returns("jwt");

        var loginDto = new LoginUserDto(
            "test@test.com", "123456"
        );
        var httpContent = loginDto.ToJsonHttpContent();

        var response = await _client.PostAsync("/api/account/login", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
    {
        var registerUser = new RegisterUserDto(
            Email: "test@test.com",
            Password: "123456",
            ConfirmPassword: "12346",
            null, null
        );

        var httpContent = registerUser.ToJsonHttpContent();

        var response = await _client.PostAsync("/api/account/register", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterUser_ForValidModel_ReturnsOk()
    {
        var registerUser = new RegisterUserDto(
            Email: "test@test.com",
            Password: "123456",
            ConfirmPassword: "123456",
            null, null
        );

        var httpContent = registerUser.ToJsonHttpContent();

        var response = await _client.PostAsync("/api/account/register", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

}