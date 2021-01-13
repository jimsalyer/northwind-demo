using Microsoft.EntityFrameworkCore;
using NorthwindDemo.Models;

namespace NorthwindDemo.Repositories
{
    public class NorthwindContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSnakeCaseNamingConvention();
    }
}
