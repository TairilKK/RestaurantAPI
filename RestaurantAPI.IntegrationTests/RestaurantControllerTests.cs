using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestaurantAPI.Dto;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;

namespace RestaurantAPI.IntegrationTests;

public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public RestaurantControllerTests(WebApplicationFactory<Program> factory)
    {
         _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));
                    services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<RestaurantDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("RestaurantDb");
                    });
                });
            });

        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task Delete_ForNoneRestaurantOwner_ReturnsForbidden()
    {
        var restaurant = await RestaurantSeed(7990);

        var response = await _client.DeleteAsync($"api/restaurant/{restaurant.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<Restaurant> RestaurantSeed(int createdById)
    {
        var restaurant = new Restaurant()
        {
            CreatedById = createdById,
            Name = "Test",
            Category = "Test Category",
            Description = "Test Description"
        };


        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<RestaurantDbContext>();

        _dbContext.Restaurants.Add(restaurant);
        await _dbContext.SaveChangesAsync();
        return restaurant;
    }

    [Fact]
    public async Task Delete_ForRestaurantOwner_ReturnsNoContent()
    {
        var restaurant = await RestaurantSeed(1);

        var response = await _client.DeleteAsync($"api/restaurant/{restaurant.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ForNonExistingRestaurant_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("api/restaurant/789");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateRestaurant_WithValidModel_ReturnsCreatedResult()
    {
        var model = new CreateRestaurantDto(
             "Pizza Yolo",
             "Pizza Yolo Desc",
             "Pizza",
             true,
            null,
            null,
            "Warsaw",
             "Długa 5",
            "11-111"
        );

        var httpContent = model.ToJsonHttpContent();
        var response = await _client.PostAsync("/api/restaurant", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }
    [Fact]
    public async Task CreateRestaurant_WithInvalidModel_ReturnsBadRequest()
    {
        var model = new CreateRestaurantDto(
            "",
            null,
            "Pizza",
            true,
            null,
            null,
            "Warsaw",
            "Długa 5",
            "11-111"
        );

        var httpContent = model.ToJsonHttpContent();
        var response = await _client.PostAsync("/api/restaurant", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("pageSize=5&pageNumber=1")]
    [InlineData("pageSize=10&pageNumber=1")]
    [InlineData("pageSize=15&pageNumber=2")]
    public async Task GetAll_WithValidQueryParameters_ReturnOkResult(string queryParams)
    {
        var response = await _client.GetAsync($"/api/restaurant?{queryParams}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("pageSize=13&pageNumber=1")]
    [InlineData("pageSize=12&pageNumber=2")]
    public async Task GetAll_WithInvalidQueryParameters_ReturnOkResult(string queryParams)
    {
        var response = await _client.GetAsync($"/api/restaurant?{queryParams}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}