using Cinex.API.Services.Interfaces;
using Cinex.Core.Entities;
using Cinex.Infrastructure.Data;

namespace Cinex.API.Services
{
    public class SessionRepository : ISessionRepository
    {
        private readonly CinexContext _context;
        public SessionRepository(CinexContext context)
        {
            _context = context;
        }
        public async Task NewSession(string sessionId, decimal amount )
        {
            var s = new Session();
            s.Id = sessionId;
            s.PaymentMode = 1;
            s.CashAmount = amount;
            s.GiftCertificateAmount = 0;
            s.CreditAmount = 0;

            _context.Set<Session>().Add(s);
            _context.SaveChanges();
        }
    }
}
