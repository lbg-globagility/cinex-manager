using Cinex.Core.Entities.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("ticket")]
    public partial class Ticket : BaseEntity
    {
        [Column("movies_schedule_list_id")]
        public int MovieScheduleListId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("terminal")]
        public string Terminal { get; set; }

        [Column("ticket_datetime")]
        public DateTime? DateTime { get; set; }

        [Column("session_id")]
        public string SessionId { get; set; }

        [Column("status")]
        public int? Status { get; set; }
    }

    public partial class Ticket
    {
        private Ticket()
        {
        }

        public virtual Session Session { get; set; }

        public virtual ICollection<MovieScheduleListReserveSeat> MovieScheduleListReserveSeats { get; set; }

        public virtual User User { get; set; }
    }
}
