using Cinex.Core.Entities;
using Cinex.Core.Entities.Base;
using Cinex.Core.Interfaces.DomainServices.Base;
using Cinex.Core.Interfaces.Repositories;
using Cinex.Core.Interfaces.Repositories.Base;
using Newtonsoft.Json;
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

            var auditTrails = new List<AuditTrail>();

            if (added?.Any() ?? false)
            {
                added.ForEach(t =>
                {
                    var detailsItems = _context.Entry(t)
                        .Properties
                        .Select(x => {
                            var propertyName = x.Metadata.Name;
                            return $"{propertyName}: {x.CurrentValue}";
                        })
                        .ToArray();

                    if (!(detailsItems?.Any() ?? false)) return;

                    var transactionDetails = $"ADDED: {string.Join(", ", detailsItems)}";

                    auditTrails.Add(AuditTrail.NewAuditTrail(userId: userId,
                        moduleCodeId: ModuleCodeId(t),
                        affectedTableLayer: $"{TableName()}",
                        transactionDetails: transactionDetails));
                });
            }

            if (updated?.Any() ?? false)
            {
                var ids = updated.GroupBy(t => t.Id)
                    .Select(t => t.Key)
                    .ToArray();

                var originalEntities = await _repository.GetManyByIdsAsync(ids);

                Func<Microsoft.EntityFrameworkCore.ChangeTracking.PropertyEntry, EntityEntryModel> selector(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> originalEntity)
                {
                    return x =>
                    {
                        var propertyName = x.Metadata.Name;
                        var originalEntityValue =
                            JsonConvert.SerializeObject(originalEntity?.Property(propertyName)?.OriginalValue);

                        var entityCurrentValue =
                            JsonConvert.SerializeObject(x.CurrentValue);

                        var hasChanged = originalEntityValue != entityCurrentValue;
                        var info = $"{propertyName}: from `{originalEntityValue}` to `{x.CurrentValue}`";

                        return new EntityEntryModel() { hasChanged = hasChanged, info = info };
                    };
                }

                updated.ForEach(t =>
                {
                    var originalEntity = _context.Entry(originalEntities?.FirstOrDefault(f => f.Id == t.Id));
                    
                    var detailsItems = _context.Entry(t)
                        .Properties
                        .Select(selector(originalEntity))
                        .Where(x => x.hasChanged)
                        .Select(x => x.info)
                        .ToArray();

                    if (!(detailsItems?.Any() ?? false)) return;

                    var transactionDetails = $"EDITED[Id:{t.Id}]: {string.Join(", ", detailsItems)}";

                    auditTrails.Add(AuditTrail.NewAuditTrail(userId: userId,
                        moduleCodeId: ModuleCodeId(t),
                        affectedTableLayer: $"{TableName()}",
                        transactionDetails: transactionDetails));
                });
            }

            if (deleted?.Any() ?? false)
            {
                deleted.ForEach(t =>
                {
                    var detailsItems = _context.Entry(t)
                        .Properties
                        .Select(x => {
                            var propertyName = x.Metadata.Name;
                            return $"{propertyName}: {x.CurrentValue}";
                        })
                        .ToArray();

                    if (!(detailsItems?.Any() ?? false)) return;

                    var transactionDetails = $"DELETED: {string.Join(", ", detailsItems)}";

                    auditTrails.Add(AuditTrail.NewAuditTrail(userId: userId,
                        moduleCodeId: ModuleCodeId(t),
                        affectedTableLayer: $"{TableName()}",
                        transactionDetails: transactionDetails));
                });
            }

            await _repository.SaveManyAsync(
                added: added,
                updated: updated,
                deleted: deleted);

            await _auditTrailRepository.SaveManyAsync(added: auditTrails);
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

        private class EntityEntryModel
        {
            public bool hasChanged { get; set; }
            public string info { get; set; }
        }
    }
}
