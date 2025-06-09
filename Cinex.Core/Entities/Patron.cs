using Cinex.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("patrons")]
    public partial class Patron : BaseEntity
    {
        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("unit_price")]
        public float UnitPrice { get; set; }

        [Column("with_promo")]
        public int? WithPromo { get; set; }

        [Column("with_amusement")]
        public int? WithAmusement { get; set; }

        [Column("with_cultural")]
        public int? WithCultural { get; set; }

        [Column("with_citytax")]
        public int? WithCityTax { get; set; }

        [Column("with_gross_margin")]
        public int? WithGrossMargin { get; set; }

        [Column("with_prod_share")]
        public int? WithProdShare { get; set; }

        [Column("seat_color")]
        public int? SeatColor { get; set; }

        [Column("seat_position")]
        public int? SeatPosition { get; set; }

        [Column("lgutax")]
        public float? LguTax { get; set; }

        [Column("base_price")]
        public int? BasePriceId { get; set; }

        [Column("with_surcharge")]
        public int? WithSurcharge { get; set; }

        [Obsolete]
        //[NotMapped]
        [Column("ewallet_id")]
        public int? EwalletId { get; private set; }
    }

    public partial class Patron
    {
        public virtual ICollection<CinemaPatron> Patrons { get; set; }

        public virtual ICollection<CinemaPatronDefault> DefaultPatrons { get; set; }

        public virtual TicketPrice TicketPrice { get; set; }

        public decimal OfficialUnitPrice => (decimal)(TicketPrice?.Price ?? UnitPrice);

        //public virtual Ewallet Ewallet { get; set; }

        public virtual ICollection<MovieScheduleListPatron> MovieScheduleListPatrons { get; set; }
    }
}
