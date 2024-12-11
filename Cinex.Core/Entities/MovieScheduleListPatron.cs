using Cinex.Core.Entities.Base;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Cinex.Core.Entities
{
    [Table("movies_schedule_list_patron")]
    public partial class MovieScheduleListPatron : BaseEntity
    {
        [Column("movies_schedule_list_id")]
        public int MovieScheduleListId { get; set; }

        [Column("patron_id")]
        public int PatronId { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("is_default")]
        public int? IsDefault { get; set; }
    }

    public partial class MovieScheduleListPatron
    {
        private MovieScheduleListPatron()
        {
        }

        public virtual ICollection<MovieScheduleListReserveSeat> MovieScheduleListReserveSeats { get; set; }

        public virtual MovieScheduleListReserveSeat MovieScheduleListReserveSeat => MovieScheduleListReserveSeats?.FirstOrDefault();

        public virtual Patron Patron { get; set; }
    }
}
