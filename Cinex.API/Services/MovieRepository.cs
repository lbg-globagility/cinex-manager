using Cinex.API.Services.Interfaces;
using Cinex.Core.Entities;
using Cinex.Infrastructure.Data;

namespace Cinex.API.Services
{
    public class MovieRepository: IMovieRepository
    {
        private readonly CinexContext _context;
        public MovieRepository(CinexContext context)
        {
            _context = context;
        }

        public async Task<Movie> MovieDetails(string code)
        {
           return _context.Set<Movie>().Where(x=>x.Code==code).FirstOrDefault();
        }
    }
}
