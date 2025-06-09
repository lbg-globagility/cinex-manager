using Cinex.Core.Entities;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
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

        public override async Task<ICollection<MovieScheduleListReserveSeat>> GetAllAsync() => await _context
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
            .ToListAsync();

        public async Task<ICollection<MovieScheduleListReserveSeat>> GetBySessionIdAsync(string sessionId) => await _context
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
            .Where(t => t.Ticket.SessionId == sessionId)
            .ToListAsync();
    }
}
