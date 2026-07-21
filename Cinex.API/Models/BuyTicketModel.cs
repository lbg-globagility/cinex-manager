using System.Collections.Generic;

namespace Cinex.API.Models
{
    public class BuyTicketModel
    {
        public decimal Amount { get; set; }

        public int CinemaUserId { get; set; }

        public int CinexUserID { get; set; }

        public int MovieScheduleListID { get; set; }

        public int PatronID { get; set; }

        public List<int> SeatIds { get; set; } = new();

        public string SessionID { get; set; } = string.Empty;

        public decimal Surcharge { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
