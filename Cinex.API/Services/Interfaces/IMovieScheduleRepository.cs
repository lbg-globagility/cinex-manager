using Cinex.Core.Entities;

namespace Cinex.API.Services.Interfaces
{
    public interface IMovieScheduleRepository
    {
        Task<List<MovieSchedule>> GetSchedules(int movieId);
        Task<MovieScheduleList> GetScheduleListByHash(string hash, int id);
    }
}
