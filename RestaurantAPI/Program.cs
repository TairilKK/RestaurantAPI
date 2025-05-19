using Microsoft.EntityFrameworkCore;
using NLog.Web;
using RestaurantAPI;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RestaurantDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RestaurantConnectionString")
    );
});
builder.Services.AddControllers();
builder.Services.AddScoped<RestaurantSeeder>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Host.UseNLog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
    seeder.Seed();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();