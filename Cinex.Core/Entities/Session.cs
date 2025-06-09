using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Cinex.Core.Entities
{
    [Table("session")]
    public partial class Session
    {
        [Column("session_id")]
        public string Id { get; set; }

        [Column("payment_mode")]
        public int PaymentMode { get; set; }

        [Column("cash_amount")]
        public decimal CashAmount { get; set; }

        [Column("gift_certificate_amount")]
        public decimal GiftCertificateAmount { get; set; }

        [Column("credit_amount")]
        public decimal CreditAmount { get; set; }
    }

    public partial class Session
    {
        private Session()
        {
        }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual ICollection<SessionEwallet> SessionEwallets { get; set; }

        public SessionEwallet SessionEwallet => SessionEwallets?.FirstOrDefault();
    }
}
