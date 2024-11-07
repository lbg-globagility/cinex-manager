using Cinex.Core.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("sound_system")]
    public partial class SoundSystem : BaseEntity
    {
        [Column("sound_system_type")]
        public string Name { get; set; }
    }

    public partial class SoundSystem
    {
        public virtual ICollection<Cinema> Cinemas { get; set; }
    }
}
