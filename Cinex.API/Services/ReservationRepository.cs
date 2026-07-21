using Cinex.API.Models;
using Cinex.API.Services.Interfaces;
using Cinex.Core.Entities;
using Cinex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cinex.API.Services
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly CinexContext _context;

        public ReservationRepository(CinexContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateReservation(BuyTicketModel model)
        {
            var ticket = new Ticket
            {
                UserId = model.CinexUserID,
                SessionId = model.SessionID,
                DateTime = System.DateTime.Now,
                Status = 1,
                MovieScheduleListReserveSeats = model.SeatIds.Select(seatId => new MovieScheduleListReserveSeat
                {
                    MovieScheduleListId = model.MovieScheduleListID,
                    CinemaSeatId = seatId,
                    PatronId = model.PatronID,
                    Price = model.UnitPrice,
                    SurchargePrice = model.Surcharge,
                    OrdinancePrice = 0,
                    Status = 1
                }).ToList()
            };

            _context.Set<Ticket>().Add(ticket);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }
    }
}
