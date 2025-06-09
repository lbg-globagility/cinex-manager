using Cinex.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.Core.Entities
{
    [Table("config_table")]
    public partial class Configuration : BaseEntity
    {
        [Column("system_code")]
        public string Code { get; set; }

        [Column("system_desc")]
        public string Description { get; set; }

        [Column("system_value")]
        public string Value { get; set; }
    }

    public partial class Configuration
    {
        public const string TABLE_NAME = "config_table";

        public const string DESCRIPTION_OR_NUMBER_FORMAT_TEXT = "OR_NUMBER_FORMAT";

        public const string DEFAULT_OR_NUMBER_FORMAT_VALUE = "B";
    }
}
