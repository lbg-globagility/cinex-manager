using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class MovieScheduleListReserveSeatRepository : SavableRepository<MovieScheduleListReserveSeat>, IMovieScheduleListReserveSeatRepository
    {
        public MovieScheduleListReserveSeatRepository(CinexContext context) : base(context)
        {
        }

        private IQueryable<MovieScheduleListReserveSeat> BaseQuery(DateTime? start = null, DateTime? end = null) => _context
            .MovieScheduleListReserveSeats
            .Include(t => t.MovieScheduleList)
                .ThenInclude(t => t.MovieSchedule)
                    .ThenInclude(t => t.Cinema)
            .Include(t => t.MovieScheduleList)
                .ThenInclude(t => t.MovieSchedule)
                    .ThenInclude(t => t.Movie)
                        .ThenInclude(t => t.Mtrcb)
            .Include(t => t.Ticket)
                .ThenInclude(t => t.Session)
            .Include(t => t.Ticket)
                .ThenInclude(t => t.User)
            .Include(t => t.CinemaSeat)
            .Include(t => t.MovieScheduleListPatron)
                .ThenInclude(t => t.Patron)
            .AsNoTracking()
            .Where(t => start==null ? true : t.MovieScheduleList.MovieSchedule.Date >= start)
            .Where(t => end == null ? true : t.MovieScheduleList.MovieSchedule.Date <= end);

        public override async Task<ICollection<MovieScheduleListReserveSeat>> GetAllAsync() => await BaseQuery()
            .ToListAsync();

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetBySessionIdAsync(string sessionId) => await BaseQuery()
            .Where(t => t.Ticket.SessionId == sessionId)
            .ToListAsync();

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetByDateRangeAsync(DateTime start, DateTime end) =>
            await BaseQuery(start: start, end: end).ToListAsync();

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetByCompositeParamsAsync(
            DateTime start,
            DateTime end,
            int[] cinemaIds = null,
            string[] usernames = null,
            string[] terminals = null,
            int[] patronIds = null)
        {
            var query = (await BaseQuery(start: start, end: end).ToListAsync())
                .Where(t => cinemaIds == null ? true : cinemaIds.Contains(t.CinemaId))
                .Where(t => usernames == null ? true : usernames.Contains(t.Username))
                .Where(t => terminals == null ? true : terminals.Contains(t.TerminalName))
                .Where(t => patronIds == null ? true : patronIds.Contains(t.PatronPriceId))
                .ToList();

            return query;
        }
    }
}
