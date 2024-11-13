
using Cinex.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("user_rights")]
    public partial class UserRight : BaseEntity
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("module_id")]
        public int ModuleId { get; set; }
    }

    public partial class UserRight
    {
        private UserRight() { }

        public virtual User User { get; set; }

        public virtual SystemModule Module { get; set; }
    }
}
