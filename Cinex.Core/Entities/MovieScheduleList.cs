using Cinex.Core.Entities.Base;
using Cinex.Core.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        public string Hash => HashString.hash(Id.ToString(), MoviesScheduleId.ToString());

        public MovieScheduleListPatron DefaultPatron => MovieScheduleListPatrons.FirstOrDefault(x => x.IsDefault);
    }

    public partial class MovieScheduleList
    {
        private MovieScheduleList()
        {
        }

        public virtual MovieSchedule MovieSchedule { get; set; }
        public virtual ICollection<MovieScheduleListPatron> MovieScheduleListPatrons { get; set; }
        public virtual ICollection<MovieScheduleListReserveSeat> MovieScheduleListReserveSeats { get; set; }
    }
}
