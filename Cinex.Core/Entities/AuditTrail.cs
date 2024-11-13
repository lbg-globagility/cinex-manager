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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tr_date")]
        public DateTime Date { get; set; }

        [Column("module_code")]
        public int ModuleCodeId { get; set; }

        [Column("aff_table_layer")]
        public string AffectedTableLayer { get; set; }

        [Column("computer_name")]
        public string ComputerName { get; set; }

        [Column("tr_details")]
        public string TransactionDetails { get; set; }
    }

    public partial class AuditTrail
    {
        private AuditTrail() { }

        public AuditTrail (int userId,
            int moduleCodeId,
            string affectedTableLayer,
            string transactionDetails,
            string computerName = "")
        {
            UserId = userId;
            ModuleCodeId = moduleCodeId;
            AffectedTableLayer = affectedTableLayer;
            ComputerName = string.IsNullOrEmpty(computerName) ? Environment.MachineName : computerName;
            TransactionDetails = transactionDetails;
        }

        public static AuditTrail NewAuditTrail(int userId,
            int moduleCodeId,
            string affectedTableLayer,
            string transactionDetails,
            string computerName = "") => new AuditTrail(userId: userId,
                moduleCodeId: moduleCodeId,
                affectedTableLayer: affectedTableLayer,
                transactionDetails: transactionDetails,
                computerName: computerName);
    }
}
