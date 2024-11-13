
using Cinex.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("user_level_rights")]
    public partial class UserLevelRight : BaseEntity
    {
        [Column("user_level")]
        public int UserLevelId { get; set; }

        [Column("module_id")]
        public int ModuleId { get; set; }

        [Column("system_code")]
        public int SystemCodeId { get; set; }
    }

    public partial class UserLevelRight
    {
        private UserLevelRight() { }

        public virtual SystemModule Module { get; set; }

        public virtual SystemCode SystemCode { get; set; }
    }
}
