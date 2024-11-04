using Cinex.Core.Entities.Base;
using Cinex.Core.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Cinex.Infrastructure.Data.Repositories.Base
{
    public abstract class SavableRepository<T> : BaseRepository, ISavableRepository<T> where T : BaseEntity
    {
        protected readonly CinexContext _context;

        public SavableRepository(CinexContext context)
        {
            _context = context;
        }

        public virtual T GetById(int id)
        {
            return _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetManyByIdsAsync(int[] ids)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task SaveAsync(T entity)
        {
            await SaveFunction(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            DetachNavigationProperties(entity);

            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            DetachNavigationProperties(entity);

            await _context.SaveChangesAsync();
        }

        public virtual async Task SaveManyAsync(List<T> entities)
        {
            await SaveManyAsync(
                added: entities.Where(x => x.IsNewEntity).ToList(),
                updated: entities.Where(x => !x.IsNewEntity).ToList());
        }

        public virtual async Task SaveManyAsync(
            List<T> added = null,
            List<T> updated = null,
            List<T> deleted = null)
        {
            if (added != null)
            {
                added.ForEach(entity =>
                {
                    if (entity.IsNewEntity) _context.Set<T>().Add(entity);
                    else _context.Entry(entity).State = EntityState.Added;

                    DetachNavigationProperties(entity);
                });
            }

            if (updated != null)
            {
                updated.ForEach(entity =>
                {
                    _context.Entry(entity).State = EntityState.Modified;
                    DetachNavigationProperties(entity);
                });
            }

            if (deleted != null)
            {
                deleted = deleted
                    .GroupBy(x => x.Id)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
                _context.Set<T>().RemoveRange(deleted);
            }

            await _context.SaveChangesAsync();
        }

        protected virtual void DetachNavigationProperties(T entity)
        {
            // no action
        }

        private async Task SaveFunction(T entity)
        {
            if (entity.IsNewEntity)
            {
                await CreateAsync(entity);
            }
            else
            {
                await UpdateAsync(entity);
            }
        }

        public async Task DeleteManyAsync(int[] ids)
        {
            var entities = await _context.Set<T>()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            _context.RemoveRange(entities);

            await _context.SaveChangesAsync();
        }
    }
}
