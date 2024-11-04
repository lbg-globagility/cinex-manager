using Cinex.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("cinema")]
    public partial class Cinema : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("sound_id")]
        public int SoundId { get; set; }

        [Column("capacity")]
        public int? Capacity { get; set; }

        [Column("in_order")]
        public int InOrder { get; set; }
    }

    public partial class Cinema
    {
        public virtual ICollection<CinemaPatron> Patrons { get; set; }

        public virtual ICollection<CinemaPatronDefault> DefaultPatrons { get; set; }

        public virtual SoundSystem SoundSystem { get; set; }
    }
}
