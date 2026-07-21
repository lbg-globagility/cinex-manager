using System.Collections.Generic;

namespace Cinex.API.Models
{
    public class ReservationDto
    {
        public int ReservationID { get; set; }

        public int UserID { get; set; }

        public int MovieScheduleListID { get; set; }

        public int PatronID { get; set; }

        public decimal TicketAmount { get; set; }

        public decimal Surcharge { get; set; }

        public string TicketSessionID { get; set; } = string.Empty;

        public List<int> SeatIds { get; set; } = new();
    }
}
