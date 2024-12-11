
using Cinex.Core.Entities.Base;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("users")]
    public partial class User : BaseEntity
    {
        [Column("userid")]
        public string UserId { get; set; }

        [Column("user_password")]
        public string Password { get; set; }

        [Column("designation")]
        public string Designation { get; set; }

        [Column("user_level_id")]
        public int UserLevelId { get; set; }

        [Column("lname")]
        public string LastName { get; set; }

        [Column("fname")]
        public string FirstName { get; set; }

        [Column("mname")]
        public string MiddleName { get; set; }

        [Column("system_code")]
        public int SystemCodeId { get; set; }

        [Column("status")]
        public int Status { get; set; }
    }

    public partial class User
    {
        private User() { }

        public virtual UserLevel UserLevel { get; set; }

        public virtual SystemCode SystemCode { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
