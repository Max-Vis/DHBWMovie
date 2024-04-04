namespace DHBWMovieDatabase.Models
{
    public interface iMoviesRepository
    {
        Task<IEnumerable<Movies>> Search(string title, string director);
        Task<Movies> GetMovies(int Id);
        Task<IEnumerable<Movies>> GetMovies();
        Task<Movies> AddMovies(Movies movies);
        Task<Movies> UpdateMovies(Movies movies);
        Task DeleteMovies(int Id);

    }
}
