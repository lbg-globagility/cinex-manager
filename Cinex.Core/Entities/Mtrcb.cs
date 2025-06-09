using Cinex.Core.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("mtrcb")]
    public partial class Mtrcb : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }
    }

    public partial class Mtrcb
    {
        private Mtrcb()
        {
        }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
