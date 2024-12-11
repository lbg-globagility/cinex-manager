using Cinex.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("session_ewallet")]
    public partial class SessionEwallet : BaseEntity
    {
        [Column("session_id")]
        public string SessionId { get; set; }

        [Column("contact_number")]
        public string ContactNo { get; set; }

        [Column("reference_number")]
        public string ReferenceNo { get; set; }

        [NotMapped]
        //[Column("ewallet_method")]
        public string EwalletMethod { get; set; }

        [Column("ewallet_id")]
        public int EwalletId { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }
    }

    public partial class SessionEwallet
    {
        public const string TABLE_NAME = "session_ewallet";

        private SessionEwallet() { }

        public SessionEwallet(string sessionId,
            string contactNo,
            string referenceNo,
            int eWalletId,
            string remarks = "")
        {
            SessionId = sessionId;
            ContactNo = contactNo;
            ReferenceNo = referenceNo;
            EwalletId = eWalletId;
            Remarks = remarks;
        }

        public static SessionEwallet New(string sessionId,
            string contactNo,
            string referenceNo,
            int eWalletId,
            string remarks = "") => new SessionEwallet(sessionId: sessionId,
                contactNo: contactNo,
                referenceNo: referenceNo,
                eWalletId: eWalletId,
                remarks: remarks);

        public virtual Session Session { get; set; }

        public virtual Ewallet Ewallet { get; set; }
    }
}
