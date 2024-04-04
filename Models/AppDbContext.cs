using Microsoft.EntityFrameworkCore;

namespace DHBWMovieDatabase.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {

        }

        public DbSet<Movies> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movies>().HasData(new Movies
            {
                Id = 1,
                Title = "Jurrasic Park",
                Director = "Stephen Spielberg",
                ReleaseDate = new DateTime(1993, 6, 11)
            });
        }

    }
}
