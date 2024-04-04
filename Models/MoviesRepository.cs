using Microsoft.EntityFrameworkCore;

namespace DHBWMovieDatabase.Models
{
    public class MoviesRepository : iMoviesRepository
    {
        private readonly AppDbContext appDbContext;

        public MoviesRepository(AppDbContext appDbContext)
        {
            this.appDbContext= appDbContext;
        }
        public async Task<Movies> AddMovies(Movies movies)
        {
            var result = await appDbContext.Movies.AddAsync(movies);
            await appDbContext.SaveChangesAsync();
            return result.Entity;

        }

        public async Task DeleteMovies(int Id)
        {
            var result = await appDbContext.Movies.FirstOrDefaultAsync(e => e.Id == Id);

            if(result != null)
            {
                appDbContext.Movies.Remove(result);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Movies> GetMovies(int Id)
        {
            return await appDbContext.Movies.FirstOrDefaultAsync(e => e.Id == Id);
        }

        public async Task<IEnumerable<Movies>> GetMovies()
        {
            return await appDbContext.Movies.ToListAsync();
        }
        
        public async Task<IEnumerable<Movies>> Search(string title, string director)
        {
            IQueryable<Movies> query = appDbContext.Movies;

            if(!string.IsNullOrEmpty(title))
            {
                query = query.Where(e => e.Title.Contains(title));
            }

            if(!string.IsNullOrEmpty(director))
            {
                query = query.Where(e => e.Director == director);
            }

            return await query.ToListAsync();
        }

        public async Task<Movies> UpdateMovies(Movies movies)
        {
            var result = await appDbContext.Movies.FirstOrDefaultAsync(e => e.Id == movies.Id);

            if(result != null)
            {
                result.Title = movies.Title;
                result.Director = movies.Director;
                result.ReleaseDate = movies.ReleaseDate;
                
                await appDbContext.SaveChangesAsync();

                return result;
            }

            return null;
        }

    }
}
