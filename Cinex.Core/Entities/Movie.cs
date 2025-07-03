using Cinex.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("movies")]
    public partial class Movie : BaseEntity
    {
        [Column("code")]
        public string Code { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("dist_id")]
        public int DistId { get; set; }

        [Column("share_perc")]
        public decimal SharePrecent { get; set; }

        [Column("rating_id")]
        public int RatingId { get; set; }

        [Column("duration")]
        public int Duration { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("encoded_date")]
        public DateTime EncodedDate { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("photo")]
        public byte[] Photo { get; set; }
    }

    public partial class Movie
    {
        private Movie()
        {
        }

        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; }

        public virtual Mtrcb Mtrcb { get; set; }
    }
}
