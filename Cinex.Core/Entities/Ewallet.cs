using Cinex.Core.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("ewallets")]
    public partial class Ewallet : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
        
        [Column("acct_no")]
        public string AccountNo { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("is_default")]
        public bool IsDefault { get; set; }
    }

    public partial class Ewallet
    {
        public const string TABLE_NAME = "ewallets";

        private Ewallet()
        {
        }

        public Ewallet(string name)
        {
            Name = name;
        }

        public static Ewallet New(string name) => new Ewallet(name);

        public virtual ICollection<Patron> Patrons { get; set; }

        public virtual ICollection<SessionEwallet> SessionEwallets { get; set; }
    }
}
