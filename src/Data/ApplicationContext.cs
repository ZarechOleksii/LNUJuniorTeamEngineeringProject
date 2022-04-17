using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<BaseEntity> BaseEntities { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreatedAsync().Wait();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
