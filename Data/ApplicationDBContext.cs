using Microsoft.EntityFrameworkCore;
using villa.Models;

namespace villa.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Sqft = 550,
                    Occupancy = 4,
                    Rate = 200.0,
                    Details = "A luxurious villa with stunning views and top-notch amenities.",
                    ImageUrl = "https://example.com/images/royal-villa.jpg",
                    CreatedDate = DateTime.Now
                },
                new Villa
                {
                    Id = 2,
                    Name = "Beachside Villa",
                    Sqft = 450,
                    Occupancy = 3,
                    Rate = 150.0,
                    Details = "A charming villa located right on the beach, perfect for a relaxing getaway.",
                    ImageUrl = "https://example.com/images/beachside-villa.jpg",
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}