using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<BaseEntity> BaseEntities { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreatedAsync().Wait();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseEntity>().HasData(
                new BaseEntity[]
                {
                new () { Id = Guid.NewGuid() },
                new () { Id = Guid.NewGuid() },
                new () { Id = Guid.NewGuid() },
                });
        }
    }
}
