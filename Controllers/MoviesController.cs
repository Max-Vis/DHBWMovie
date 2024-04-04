using DHBWMovieDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DHBWMovieDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly iMoviesRepository moviesRepository;

        public MoviesController(iMoviesRepository moviesRepository)
        {
            this.moviesRepository = moviesRepository;
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Movies>>> Search(string title, string director)
        {
            try
            {
                var result = await moviesRepository.Search(title, director);

                if(result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Data from the Database");
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetMovies()
        {
            try
            {
                return Ok(await moviesRepository.GetMovies());

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Movies>> GetMovies(int id)
        {
            try
            {
                var result = await moviesRepository.GetMovies(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]

        public async Task<ActionResult<Movies>> CreateMovies(Movies movies)
        {
            try
            {
                if (movies == null)
                    return BadRequest();
                var tempM = await moviesRepository.GetMovies(movies.Id);

                if (tempM != null)
                {
                    ModelState.AddModelError("Id", "Id already used");
                }

                var createdMovie = await moviesRepository.AddMovies(movies);

                return CreatedAtAction(nameof(GetMovies),new {id = createdMovie.Id}, createdMovie);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new database item");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Movies>> UpdateMovies(int id, Movies movies)
        {
            try
            {
                if(id != movies.Id)
                    return BadRequest("Id mismatch");
                    var movieToUpdate = await moviesRepository.GetMovies(id);

                    if (movieToUpdate == null)
                    {
                        return NotFound($"Movie with Id = {id} not found");
                    }
                    return await moviesRepository.UpdateMovies(movies);
                
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating movie entry");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMovies(int id)
        {
            try
            {
                var movieToDelete = await moviesRepository.GetMovies(id);

                if (movieToDelete == null)
                {
                    return NotFound($"Movie with Id = {id} not found");
                }

                await moviesRepository.DeleteMovies(id);

                return Ok($"Movie with Id = {id} deleted");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting movie from database");
            }
        }
    }
}
