using Cinex.Core.Entities;
using Cinex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinex.API.Services
{
    /// <summary>
    /// Periodically pushes reserved seats that haven't been synced yet (is_sync = 0) from the
    /// local db to the online db, marking each one as synced (is_sync = 1) once it lands there.
    /// </summary>
    public class ReserveSeatSyncService : BackgroundService
    {
        private const int BatchSize = 50;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReserveSeatSyncService> _logger;
        private readonly TimeSpan _interval;

        public ReserveSeatSyncService(
            IServiceScopeFactory scopeFactory,
            ILogger<ReserveSeatSyncService> logger,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            var seconds = configuration.GetValue<int?>("ReserveSeatSync:IntervalSeconds") ?? 20;
            _interval = TimeSpan.FromSeconds(seconds);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_interval);

            do
            {
                try
                {
                     SyncPendingSeatsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Reserved seat sync run failed.");
                }
            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }

        private async Task SyncPendingSeatsAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var localContext = scope.ServiceProvider.GetRequiredService<CinexContext>();
            var onlineContext = scope.ServiceProvider.GetRequiredService<OnlineCinexContext>();

            var pending = await localContext.Set<MovieScheduleListReserveSeat>()
                .Where(x => !x.IsSync)
                .OrderByDescending(x => x.Id)
                .Take(BatchSize)
                .ToListAsync(stoppingToken);

            if (pending.Count == 0)
            {
                return;
            }

            _logger.LogInformation("Syncing {Count} reserved seat(s) to the online database.", pending.Count);

            foreach (var seat in pending)
            {
                stoppingToken.ThrowIfCancellationRequested();

                try
                {
                    var alreadyOnline = await onlineContext.Set<MovieScheduleListReserveSeat>()
                        .AsNoTracking()
                        .AnyAsync(x => x.Id == seat.Id, stoppingToken);

                    if (!alreadyOnline)
                    {
                        onlineContext.Set<MovieScheduleListReserveSeat>().Add(ToOnlineEntity(seat));
                        await onlineContext.SaveChangesAsync(stoppingToken);
                    }

                    seat.IsSync = true;
                    await localContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to sync reserved seat {Id}.", seat.Id);
                }
                finally
                {
                    DetachAll(onlineContext);
                }
            }
        }

        private static void DetachAll(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }

        private static MovieScheduleListReserveSeat ToOnlineEntity(MovieScheduleListReserveSeat source) => new()
        {
            Id = source.Id,
            MovieScheduleListId = source.MovieScheduleListId,
            CinemaSeatId = source.CinemaSeatId,
            TicketId = source.TicketId,
            PatronId = source.PatronId,
            Price = source.Price,
            BasePrice = source.BasePrice,
            Status = source.Status,
            AmusementTaxAmount = source.AmusementTaxAmount,
            CulturalTaxAmount = source.CulturalTaxAmount,
            VatAmount = source.VatAmount,
            ORNumber = source.ORNumber,
            VoidUserId = source.VoidUserId,
            VoidDateTime = source.VoidDateTime,
            OrdinancePrice = source.OrdinancePrice,
            SurchargePrice = source.SurchargePrice,
            IsSync = true
        };
    }
}
