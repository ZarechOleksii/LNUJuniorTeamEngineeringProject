using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data
{
    public class ApplicationContext : IdentityDbContext
    {
        public DbSet<BaseEntity> BaseEntities { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Favourites> Favourites { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeletedAsync().Wait();
            //Database.EnsureCreatedAsync().Wait();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favourites>()
                .HasIndex(e => new { e.MovieId, e.UserId })
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
