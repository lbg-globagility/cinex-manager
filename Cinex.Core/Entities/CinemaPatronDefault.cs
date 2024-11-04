using Cinex.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("cinema_patron_default")]
    public partial class CinemaPatronDefault : BaseEntity
    {
        [Column("cinema_id")]
        public int? CinemaId { get; set; }

        [Column("patron_id")]
        public int? PatronId { get; set; }
    }

    public partial class CinemaPatronDefault
    {
        public virtual Cinema Cinema { get; set; }

        public virtual Patron Patron { get; set; }
    }
}
