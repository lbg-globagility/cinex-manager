using Cinex.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("system_module")]
    public partial class SystemModule : BaseEntity
    {
        [Column("module_code")]
        public string Code { get; set; }

        [Column("module_desc")]
        public string Description { get; set; }

        [Column("module_group")]
        public string Group { get; set; }

        [Column("system_code")]
        public int SystemCodeId { get; set; }
    }

    public partial class SystemModule
    {
        public const string CINEMA_ADD_CODE_TEXT = "CINEMA_ADD";
        public const string CINEMA_EDIT_CODE_TEXT = "CINEMA_EDIT";
        public const string CINEMA_DELETE_CODE_TEXT = "CINEMA_DELETE";
        public const string CINEMA_CODE_TEXT = "CINEMA";

        public const string PATRON_ADD_CODE_TEXT = "PATRON_ADD";
        public const string PATRON_EDIT_CODE_TEXT = "PATRON_EDIT";
        public const string PATRON_DELETE_CODE_TEXT = "PATRON_DELETE";

        public const string RESERVE_CODE_TEXT = "RESERVE";

        private SystemModule() { }

        public virtual SystemCode SystemCode { get; set; }
    }
}
