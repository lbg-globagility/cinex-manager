using Cinex.API.Services.Interfaces;
using Cinex.Core.Entities;
using Cinex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinex.API.Services
{
    public class MovieScheduleRepository: IMovieScheduleRepository
    {
        private readonly CinexContext _context;
        public MovieScheduleRepository(CinexContext context)
        {
            _context = context;
        }
        public async Task<List<MovieSchedule>> GetSchedules(int movieId)
        {
            var list = _context.Set<MovieSchedule>().Include(x => x.MovieScheduleLists).OrderBy(x => x.Date).Where(x => x.MovieId == movieId).ToList();
            return list;
        }
        public async Task<MovieScheduleList> GetScheduleListByHash(string hash, int id)
        {
            var details = _context.Set<MovieScheduleList>()
                    .Include(x => x.MovieSchedule)
                        .ThenInclude(x => x.Cinema)
                            .ThenInclude(x => x.Seats)
                    .Include(x => x.MovieScheduleListPatrons).Where(x=>x.Id==id);
            var q = details.AsEnumerable().FirstOrDefault(x => x.Hash == hash );
            var occupiedSeat = _context.Set<MovieScheduleListReserveSeat>().Where(b => b.MovieScheduleListId == id && b.VoidUserId==null).ToList();


            q.MovieSchedule.Cinema.Seats = q.MovieSchedule.Cinema.Seats.Select(x =>
            {
                x.IsOccupied = occupiedSeat.Where(b => x.Id == b.CinemaSeatId).Any();
                return x;
            }).ToList();
            return q;

        }
    }
}
