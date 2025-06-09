using Cinex.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("movies_schedule_list")]
    public partial class MovieScheduleList : BaseEntity
    {
        [Column("movies_schedule_id")]
        public int MoviesScheduleId { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("seat_type")]
        public int SeatType { get; set; }

        [Column("laytime")]
        public int LayTime { get; set; }

        [Column("status")]
        public int Status { get; set; }
    }

    public partial class MovieScheduleList
    {
        private MovieScheduleList()
        {
        }

        public virtual MovieSchedule MovieSchedule { get; set; }

        public virtual ICollection<MovieScheduleListReserveSeat> MovieScheduleListReserveSeats { get; set; }
    }
}
