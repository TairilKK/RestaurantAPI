using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RestaurantAPI.IntegrationTests;

public class RestaurantControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

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