using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinex.Core.Interfaces.Repositories
{
    public interface IAuditTrailRepository
    {
        Task CreateRecordAsync();

        //Task<PaginatedList<UserActivityItem>> GetPaginatedListAsync(PageOptions options, int? organizationId = null, string[] entityNames = null, int? changedByUserId = null, UserActivity.ChangedType? changedType = null, int? changedEntityId = null, string description = null, DateTime? dateFrom = null, DateTime? dateTo = null);

        Task RecordAddAsync();

        Task RecordDeleteAsync();
    }
}
