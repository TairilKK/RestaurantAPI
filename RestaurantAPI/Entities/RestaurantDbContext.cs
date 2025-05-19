using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Entities;

public class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options)
{
    public DbSet<Adress> Adresses { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Dish> Dishes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>(eb =>
        {
            eb.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(30);
        });

        modelBuilder.Entity<Adress>(eb =>
        {
            eb.HasOne(a => a.Restaurant)
                .WithOne(r => r.Adress)
                .HasForeignKey<Restaurant>(r => r.AddressId);

            eb.Property(a => a.City)
                .HasMaxLength(50);

            eb.Property(a => a.Street)
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Dish>(eb =>
        {
            eb.Property(e => e.Name)
                .IsRequired();
        });
    }
}