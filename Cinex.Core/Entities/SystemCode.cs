using Cinex.Core.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("application")]
    public partial class SystemCode : BaseEntity
    {
        [Column("system_code")]
        public int Code { get; set; }

        [Column("system_name")]
        public string Name { get; set; }
    }

    public partial class SystemCode
    {
        public const string CINEMA_MANAGER_TEXT = "CINEMA MANAGER";
        public const string CINEMA_TICKETING_TEXT = "CINEMA TICKETING SYSTEM";

        private SystemCode() { }

        public virtual ICollection<SystemModule> Modules { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<UserLevel> UserLevels { get; set; }

        public virtual ICollection<UserLevelRight> UserLevelRights { get; set; }
    }
}
