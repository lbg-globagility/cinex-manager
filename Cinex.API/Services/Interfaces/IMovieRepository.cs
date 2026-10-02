using Cinex.Core.Entities;

namespace Cinex.API.Services.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie> MovieDetails(string code);
    }
}
