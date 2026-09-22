using Cinex.API.Models;
using Cinex.API.Services.Interfaces;
using Cinex.Core.Entities;
using Cinex.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
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
        
        public async Task<bool> CheckIfTheSeatIsAvailable(int modelScheduleListID, List<int> seatIds)
        {
            bool isSuccess = false;
            var existingReservations = await _context.Set<MovieScheduleListReserveSeat>()
       .AnyAsync(x => x.MovieScheduleListId == modelScheduleListID
                   && seatIds.Contains(x.CinemaSeatId)
                   && x.Status == 1);

            if (existingReservations)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> CreateReservation(BuyTicketModel model)
        {
            bool isSuccess = false;
            var isAvailable = await this.CheckIfTheSeatIsAvailable(model.MovieScheduleListID, model.SeatIds);
            if (!isAvailable)
            {
                return false;
            }
            var latestRsveseat = _context.Set<MovieScheduleListReserveSeat>().OrderByDescending(x => x.Id).FirstOrDefault();
            var latestOR = latestRsveseat.ORNumber;
            long.TryParse(latestOR, out long orCounter);
            var ticket = new Ticket
            {
                UserId = model.CinexUserID,
                SessionId = model.SessionID,
                DateTime = System.DateTime.Now,
                Status = 1,
                MovieScheduleListId = model.MovieScheduleListID,

                // Use a multi-line statement block inside Select to safely increment the number
                MovieScheduleListReserveSeats = model.SeatIds.Select(seatId =>
                {
                    orCounter++; // Move to the next sequence number

                    return new MovieScheduleListReserveSeat
                    {
                        MovieScheduleListId = model.MovieScheduleListID,
                        CinemaSeatId = seatId,
                        PatronId = model.PatronID,
                        Price = model.UnitPrice,
                        SurchargePrice = model.Surcharge,
                        OrdinancePrice = 0,
                        Status = 1,
                        AmusementTaxAmount = 0,
                        CulturalTaxAmount = 0,
                        BasePrice= model.UnitPrice,
                        IsSync=false,
                        ORNumber = orCounter.ToString("D9") // Generates padded string (e.g., "000265998")
                    };
                }).ToList()
            };

            _context.Set<Ticket>().Add(ticket);
            var affected = await _context.SaveChangesAsync();
            return true;
        }
    }
}
