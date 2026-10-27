using Cinex.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("movies_schedule")]
    public partial class MovieSchedule : BaseEntity
    {
        [Column("cinema_id")]
        public int CinemaId { get; set; }

        [Column("movie_id")]
        public int MovieId { get; set; }

        [Column("movie_date")]
        public DateTime Date { get; set; }
    }

    public partial class MovieSchedule
    {
        public MovieSchedule()
        {
        }

        public virtual ICollection<MovieScheduleList> MovieScheduleLists { get; set; }

        public virtual Cinema Cinema { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
