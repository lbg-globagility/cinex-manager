using Cinex.Core.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Cinex.Infrastructure.Data.Repositories
{
    public class AuditTrailRepository : IAuditTrailRepository
    {
        private readonly CinexContext _context;

        public AuditTrailRepository(CinexContext context)
        {
            _context = context;
        }

        public Task CreateRecordAsync()
        {
            throw new NotImplementedException();
        }

        public Task RecordAddAsync()
        {
            throw new NotImplementedException();
        }

        public Task RecordDeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
