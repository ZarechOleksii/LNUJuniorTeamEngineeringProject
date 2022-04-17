using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class ApplicationContext : IdentityDbContext
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
            base.OnModelCreating(modelBuilder);
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
