using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cinex.Core.Entities.Base
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        /// <summary>
        /// Checks if the entity is new (not yet in the database), by checking its RowID if it is less than or equal to 0.
        /// </summary>
        public bool IsNewEntity => CheckIfNewEntity(Id);

        /// <summary>
        /// Checks if the entity is new (not yet in the database), by checking its RowID if it is less than or equal to 0.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckIfNewEntity(int? id)
        {
            // sometimes it's not int.MinValue
            return id == null || id <= 0;
        }

        [NotMapped]
        public bool IsDelete { get; private set; }

        public void SetDelete()
        {
            IsDelete = true;
        }

        [NotMapped]
        public bool IsEdited { get; private set; }

        public void SetEdited()
        {
            IsEdited = true;
        }
    }
}
