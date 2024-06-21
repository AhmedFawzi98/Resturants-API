using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Resturants.Domain.Entities;

namespace Resturants.Infrastructure.Persistance;

internal class RestaurantDbContext:IdentityDbContext<ApplicationUser>
{
    public RestaurantDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Restaurant> Restaurants { get; set; }
    public virtual DbSet<Dish> Dishes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>(bldr =>
        {
            bldr.HasMany(r => r.Dishes)
                .WithOne(d => d.Restaurant)
                .HasForeignKey(d => d.RestaurantId);

            bldr.HasOne(r => r.Owner)
                .WithMany(o => o.OwnedRestaurants)
                .HasForeignKey(r => r.OwnerId);

            bldr.OwnsOne(r => r.Address,
                address=> address.Property(a => a.PostalCode).HasMaxLength(10));

            bldr.Property(r => r.Name).HasMaxLength(150);
            bldr.Property(r => r.Category).HasMaxLength(150);


        });
        base.OnModelCreating(modelBuilder);
    }


}
