using Cinex.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("ticket_prices")]
    public partial class TicketPrice : BaseEntity
    {
        [Column("price")]
        public double Price { get; set; }

        [Column("effective_date")]
        public DateTime EffectiveDate { get; set; }
    }

    public partial class TicketPrice
    {
        public virtual ICollection<Patron> Patrons { get; set; }
    }
}
