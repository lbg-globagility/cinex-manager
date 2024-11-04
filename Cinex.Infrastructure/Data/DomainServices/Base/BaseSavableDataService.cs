using Cinex.Core.Entities.Base;
using Cinex.Core.Interfaces.DomainServices.Base;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Core.Interfaces.Repositories.Base;

namespace Cinex.Infrastructure.Data.DomainServices.Base
{
    public abstract class BaseSavableDataService<T> : BaseDataService, IBaseSavableDataService<T> where T : BaseEntity
    {
        private readonly ISavableRepository<T> _repository;
        private readonly CinexContext _context;

        protected BaseSavableDataService(
            ISavableRepository<T> repository,
            IAuditTrailRepository auditTrailRepository,
            CinexContext context) :
            base(auditTrailRepository)
        {
            _repository = repository;
            _context = context;
        }

        public Task DeleteAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync(int[] ids, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(int id) => await _repository.GetByIdAsync(id: id);

        public Task<T> SaveAsync(T entity, int userId)
        {
            throw new NotImplementedException();
        }

        public Task SaveManyAsync(int userId, List<T> added = null, List<T> updated = null, List<T> deleted = null)
        {
            throw new NotImplementedException();
        }

        public Task SaveManyAsync(List<T> entities, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
