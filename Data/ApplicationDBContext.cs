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
    }
}