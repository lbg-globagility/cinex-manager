using Cinex.Core.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("cinema_seat")]
    public partial class CinemaSeat : BaseEntity
    {
        [Column("cinema_id")]
        public int CinemaId { get; set; }

        [Column("x1")]
        public int? X1 { get; set; }

        [Column("y1")]
        public int? Y1 { get; set; }

        [Column("x2")]
        public int? X2 { get; set; }

        [Column("y2")]
        public int? Y2 { get; set; }

        [Column("origin_x")]
        public int? OriginX { get; set; }

        [Column("origin_y")]
        public int? OriginY { get; set; }

        [Column("row_name")]
        public string RowName { get; set; }

        [Column("col_name")]
        public string ColumnName { get; set; }

        [Column("object_type")]
        public int? ObjectType { get; set; }

        [Column("is_handicapped")]
        public bool IsHandicapped { get; set; }

        [Column("group_name")]
        public string GroupNamme { get; set; }

        [Column("section_name")]
        public string SectionName { get; set; }

        [Column("is_disabled")]
        public bool IsDisabled { get; set; }
    }

    public partial class CinemaSeat
    {
        private CinemaSeat()
        {
        }

        public virtual ICollection<MovieScheduleListReserveSeat> MovieScheduleListReserveSeats { get; set; }
    }
}
