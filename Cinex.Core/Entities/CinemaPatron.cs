using Cinex.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("cinema_patron")]
    public partial class CinemaPatron : BaseEntity
    {
        [Column("cinema_id")]
        public int CinemaId { get; set; }

        [Column("patron_id")]
        public int PatronId { get; set; }

        [Column("price")]
        public float Price { get; set; }
    }

    public partial class CinemaPatron
    {
        public virtual Cinema Cinema { get; set; }

        public virtual Patron Patron { get; set; }

        private CinemaPatron() { }

        public CinemaPatron(
            int cinemaId,
            int patronId,
            double price)
        {
            CinemaId = cinemaId;
            PatronId = patronId;
            Price = (float)price;
        }

        public static CinemaPatron New(
            int cinemaId,
            int patronId,
            double price) => new CinemaPatron(
                cinemaId: cinemaId,
                patronId: patronId,
                price: price);
    }
}
