using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using RestaurantAPI.Controllers;

namespace RestaurantAPI.IntegrationTests;

public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly List<Type> _controllerTypes;

    public ProgramTests(WebApplicationFactory<Program> factory)
    {
        _controllerTypes = typeof(Program)
            .Assembly
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
            .ToList();


        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                _controllerTypes.ForEach(c => services.AddScoped(c));
            });
        });
    }
    [Fact]
    public void ConfigureProgram_ForControllers_RegistersAllDependencies()
    {
        var scopeFactor = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactor!.CreateScope();

        _controllerTypes.ForEach(t =>
        {
            var controller = scope.ServiceProvider.GetService<AccountController>();
            controller.Should().NotBeNull();
        });

    }
}