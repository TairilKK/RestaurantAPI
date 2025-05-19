using RestaurantAPI.Entities;

namespace RestaurantAPI;

public class RestaurantSeeder(RestaurantDbContext _dbContext)
{
    public void Seed()
    {
        if (!_dbContext.Database.CanConnect()) return;
        if (_dbContext.Restaurants.Any()) return;

        var restaurants = GetRestaurants();
        _dbContext.Restaurants.AddRange(restaurants);
        _dbContext.SaveChanges();
    }

    private List<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>()
        {
            new Restaurant()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description =
                    "KFC is a global fast-food chain known for its fried chicken, founded by Harland Sanders in 1952, and is headquartered in Louisville, Kentucky.",
                HasDelivery = true,
                ContactEmail = "kfc@email.com",
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name = "Nashville Hot Chicken",
                        Price = 10.30M
                    },
                    new Dish()
                    {
                        Name = "Chicken Nugets",
                        Price = 5.30M
                    },
                },
                Adress = new Adress()
                {
                    City = "Warszawa",
                    Street = "Szeroka 5",
                    PostalCode = "30-001"
                }
            },
            new Restaurant()
            {
                Name = "Sushi World",
                Category = "Japanese",
                Description =
                    "Sushi World offers a wide variety of traditional and modern sushi dishes, using only the freshest ingredients imported from Japan.",
                HasDelivery = true,
                ContactEmail = "shushi_world@email.com",
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name = "California Roll",
                        Price = 18.50M
                    },
                    new Dish()
                    {
                        Name = "Nigiri Salmon",
                        Price = 22.00M
                    },
                },
                Adress = new Adress()
                {
                    City = "Kraków",
                    Street = "Rynek Główny 12",
                    PostalCode = "31-042"
                }
            },

            new Restaurant()
            {
                Name = "Pizzeria Bella Italia",
                Category = "Italian",
                Description =
                    "Pizzeria Bella Italia serves authentic Italian pizzas baked in a traditional wood-fired oven, alongside classic pastas and wines.",
                HasDelivery = false,
                ContactEmail = "bella_italia@email.com",
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name = "Margherita",
                        Price = 16.90M
                    },
                    new Dish()
                    {
                        Name = "Quattro Formaggi",
                        Price = 21.50M
                    },
                },
                Adress = new Adress()
                {
                    City = "Gdańsk",
                    Street = "Długa 22",
                    PostalCode = "80-827"
                }
            }
        };
    }
}