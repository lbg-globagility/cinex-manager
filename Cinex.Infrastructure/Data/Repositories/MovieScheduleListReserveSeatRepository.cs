using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class MovieScheduleListReserveSeatRepository : SavableRepository<MovieScheduleListReserveSeat>, IMovieScheduleListReserveSeatRepository
    {
        public MovieScheduleListReserveSeatRepository(CinexContext context) : base(context)
        {
        }

        private IQueryable<MovieScheduleListReserveSeat> BaseQuery => _context
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
            .AsNoTracking();

        public override async Task<ICollection<MovieScheduleListReserveSeat>> GetAllAsync() => await BaseQuery
            .ToListAsync();

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetBySessionIdAsync(string sessionId) => await BaseQuery
            .Where(t => t.Ticket.SessionId == sessionId)
            .ToListAsync();

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetByDateRangeAsync(DateTime start, DateTime end) =>
            await BaseQuery
                .Where(t => t.Ticket.Date >= start)
                .Where(t => t.Ticket.Date <= end)
                .ToListAsync();

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetByCompositeParamsAsync(
            DateTime start,
            DateTime end,
            int[] cinemaIds = null,
            string[] usernames = null,
            string[] terminals = null,
            int[] patronIds = null)
        {
            var query = BaseQuery
                .AsEnumerable()
                .Where(t => t.DateTime.Value.Date >= start)
                .Where(t => t.DateTime.Value.Date <= end);

            if ((cinemaIds?.Any() ?? false))
                query = query.Where(t => cinemaIds.Contains(t.CinemaId));

            if ((usernames?.Any() ?? false))
                query = query.Where(t => usernames.Contains(t.Username));

            if ((terminals?.Any() ?? false))
                query = query.Where(t => terminals.Contains(t.TerminalName));

            if ((patronIds?.Any() ?? false))
                query = query.Where(t => patronIds.Contains(t.PatronPriceId));

            return await Task.FromResult(query
                .AsEnumerable()
                .ToList());
        }
    }
}
