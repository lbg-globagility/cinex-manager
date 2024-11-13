using Cinex.Core.Entities;
using Cinex.Core.Entities.Base;
using Cinex.Core.Interfaces.DomainServices.Base;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Core.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.DomainServices.Base
{
    public abstract class BaseSavableDataService<T> : BaseDataService, IBaseSavableDataService<T> where T : BaseEntity
    {
        private readonly ISavableRepository<T> _repository;
        private readonly CinexContext _context;

        protected BaseSavableDataService(
            ISavableRepository<T> repository,
            IAuditTrailRepository auditTrailRepository,
            ISystemModuleRepository systemModuleRepository,
            CinexContext context) :
            base(auditTrailRepository, systemModuleRepository)
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

        public async Task SaveManyAsync(
            int userId,
            List<T> added = null,
            List<T> updated = null,
            List<T> deleted = null)
        {

            var auditTrail = new List<AuditTrail>();

            if (added?.Any() ?? false)
            {
                var addedEntityEntry = _context.Entry(added?.FirstOrDefault());
                added.ForEach(t =>
                {
                    var detailsItems = addedEntityEntry
                        .Properties
                        .Select(x => {
                            var propertyName = x.Metadata.Name;
                            return $"{propertyName}: {x.CurrentValue}";
                        })
                        .ToArray();

                    var transactionDetails = string.Join(", ", detailsItems);

                    auditTrail.Add(AuditTrail.NewAuditTrail(userId: userId,
                        moduleCodeId: ModuleCodeId(t),
                        affectedTableLayer: $"{TableName()}",
                        transactionDetails: transactionDetails));
                });
            }

            if (updated?.Any() ?? false)
            {
                var updatedEntityEntry = _context.Entry(updated?.FirstOrDefault());
                updated.ForEach(t =>
                {
                    var detailsItems = updatedEntityEntry
                        .Properties
                        .Select(x => {
                            var propertyName = x.Metadata.Name;
                            return $"{propertyName}: {x.CurrentValue}";
                        })
                        .ToArray();

                    var transactionDetails = string.Join(", ", detailsItems);

                    auditTrail.Add(AuditTrail.NewAuditTrail(userId: userId,
                        moduleCodeId: ModuleCodeId(t),
                        affectedTableLayer: $"{TableName()}",
                        transactionDetails: transactionDetails));
                });
            }

            if (deleted?.Any() ?? false)
            {
                var deletedEntityEntry = _context.Entry(deleted?.FirstOrDefault());
                deleted.ForEach(t =>
                {
                    var detailsItems = deletedEntityEntry
                        .Properties
                        .Select(x => {
                            var propertyName = x.Metadata.Name;
                            return $"{propertyName}: {x.CurrentValue}";
                        })
                        .ToArray();

                    var transactionDetails = string.Join(", ", detailsItems);

                    auditTrail.Add(AuditTrail.NewAuditTrail(userId: userId,
                        moduleCodeId: ModuleCodeId(t),
                        affectedTableLayer: $"{TableName()}",
                        transactionDetails: transactionDetails));
                });
            }

            await _repository.SaveManyAsync(
                added: added,
                updated: updated,
                deleted: deleted);

            await _auditTrailRepository.SaveManyAsync(added: auditTrail);
        }

        public async Task SaveManyAsync(
            List<T> entities,
            int userId)
        {
            var insertEntities = entities.Where(x => x.IsNewEntity).ToList();
            var updateEntities = entities.Where(x => x.IsEdited).ToList();
            var deleteEntities = entities.Where(x => x.IsDelete).ToList();

            await SaveManyAsync(
                userId,
                added: insertEntities,
                updated: updateEntities,
                deleted: deleteEntities);
        }

        protected abstract string TableName(T entity = null);

        protected abstract int ModuleCodeId(T entity = null);
    }
}
