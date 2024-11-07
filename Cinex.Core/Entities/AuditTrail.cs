using Cinex.Core.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("a_trail")]
    public partial class AuditTrail : BaseEntity
    {
        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("tr_date")]
        public DateTime Date { get; set; }

        [Column("module_code")]
        public int ModuleCode { get; set; }

        [Column("aff_table_layer")]
        public string AffectedTableLayer { get; set; }

        [Column("computer_name")]
        public string ComputerName { get; set; }

        [Column("tr_details")]
        public string TransactionDetails { get; set; }
    }
}
