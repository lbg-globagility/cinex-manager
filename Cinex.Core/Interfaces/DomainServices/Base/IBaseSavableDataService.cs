using Cinex.Core.Entities.Base;

namespace Cinex.Core.Interfaces.DomainServices.Base
{
    public interface IBaseSavableDataService<T> : IBaseDataService where T : BaseEntity
    {
        Task DeleteAsync(int id, int userId);

        Task DeleteManyAsync(int[] ids, int userId);

        Task<T> SaveAsync(T entity, int userId);

        Task SaveManyAsync(int userId,
            List<T> added = null,
            List<T> updated = null,
            List<T> deleted = null);

        Task SaveManyAsync(List<T> entities, int userId);

        Task<T> GetByIdAsync(int id);
    }
}
