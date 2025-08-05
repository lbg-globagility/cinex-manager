using Cinex.Core.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("movies_schedule_list_reserved_seat")]
    public partial class MovieScheduleListReserveSeat : BaseEntity
    {
        [Column("movies_schedule_list_id")]
        public int MovieScheduleListId { get; set; }

        [Column("cinema_seat_id")]
        public int CinemaSeatId { get; set; }

        [Column("ticket_id")]
        public int TicketId { get; set; }

        [Column("patron_id")]
        public int PatronId { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("base_price")]
        public decimal? BasePrice { get; set; }

        [Column("status")]
        public int? Status { get; set; }

        [Column("amusement_tax_amount")]
        public decimal? AmusementTaxAmount { get; set; }

        [Column("cultural_tax_amount")]
        public decimal? CulturalTaxAmount { get; set; }

        [Column("vat_amount")]
        public decimal? VatAmount { get; set; }

        [Column("or_number")]
        public string ORNumber { get; set; }

        [Column("void_user_id")]
        public int? VoidUserId { get; set; }

        [Column("void_datetime")]
        public DateTime? VoidDateTime { get; set; }

        [Column("ordinance_price")]
        public decimal OrdinancePrice { get; set; }

        [Column("surcharge_price")]
        public decimal SurchargePrice { get; set; }
    }

    public partial class MovieScheduleListReserveSeat
    {
        private MovieScheduleListReserveSeat()
        {
        }

        public virtual Ticket Ticket { get; set; }

        public virtual MovieScheduleList MovieScheduleList { get; set; }

        public virtual CinemaSeat CinemaSeat { get; set; }

        public virtual MovieScheduleListPatron MovieScheduleListPatron { get; set; }

        public DateTime? DateTime => Ticket?.DateTime;

        public DateTime? Date => Ticket?.Date;

        public int CinemaId => MovieScheduleList?.MovieSchedule?.CinemaId ?? 0;

        public string CinemaName => MovieScheduleList?.MovieSchedule?.Cinema?.Name;

        public string Username => Ticket?.User?.UserId;

        public string TerminalName => Ticket?.Terminal;

        public int PatronPriceId => MovieScheduleListPatron?.PatronId ?? 0;

        public string PatronCode => MovieScheduleListPatron?.Patron?.Code;

        public string PatronName => MovieScheduleListPatron?.Patron?.Name;
    }
}
