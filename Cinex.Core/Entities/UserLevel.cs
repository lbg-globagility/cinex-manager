
using Cinex.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("user_level")]
    public partial class UserLevel : BaseEntity
    {
        [Column("level_desc")]
        public string Name { get; set; }

        [Column("system_code")]
        public int SystemCodeId { get; set; }
    }

    public partial class UserLevel
    {
        private UserLevel() { }

        public virtual SystemCode SystemCode { get; set; }
    }
}
