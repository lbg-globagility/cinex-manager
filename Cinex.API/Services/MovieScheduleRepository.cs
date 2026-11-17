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

        /// <summary>
        /// Schedules for a movie, carrying only the screenings that start at or after
        /// <paramref name="screeningsFrom"/>. A schedule with no remaining screenings is
        /// still returned, with an empty screening list.
        /// </summary>
        public async Task<List<MovieSchedule>> GetSchedules(int movieId, DateTime screeningsFrom)
        {
            var fromDate = screeningsFrom.Date;

            // EF Core 3.1 has no filtered Include (that arrived in 5.0), so the screenings are
            // filtered through a projection and left to navigation fixup to attach to the parent.
            // This relies on change tracking - do not add AsNoTracking here.
            var rows = await _context.Set<MovieSchedule>()
                .Where(x => x.MovieId == movieId && x.Date >= fromDate)
                .OrderBy(x => x.Date)
                .Select(x => new
                {
                    Schedule = x,
                    Screenings = x.MovieScheduleLists
                        .Where(l => l.StartTime >= screeningsFrom)
                        .OrderBy(l => l.StartTime)
                        .ToList()
                })
                .ToListAsync();

            return rows.Select(x => x.Schedule).ToList();
        }

        public async Task<MovieScheduleList> GetScheduleListByHash(string hash, int id)
        {
            var details = await _context.Set<MovieScheduleList>()
                    .Include(x => x.MovieSchedule)
                        .ThenInclude(x => x.Cinema)
                            .ThenInclude(x => x.Seats)
                    .Include(x => x.MovieScheduleListPatrons).Where(x=>x.Id==id)
                    .ToListAsync();

            // Hash is a computed property with no column, so it can only be matched client side.
            var q = details.FirstOrDefault(x => x.Hash == hash );

            var occupiedSeat = await _context.Set<MovieScheduleListReserveSeat>()
                .Where(b => b.MovieScheduleListId == id && b.VoidUserId==null)
                .ToListAsync();

            q.MovieSchedule.Cinema.Seats = q.MovieSchedule.Cinema.Seats.Select(x =>
            {
                x.IsOccupied = occupiedSeat.Where(b => x.Id == b.CinemaSeatId).Any();
                return x;
            }).ToList();
            return q;

        }
    }
}
