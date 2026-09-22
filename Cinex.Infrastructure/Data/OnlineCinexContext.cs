using Cinex.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinex.Infrastructure.Data
{
    public class OnlineCinexContext : CinexContext
    {
        public OnlineCinexContext(DbContextOptions<OnlineCinexContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Reserved seats are synced from the local db with their id already assigned,
            // so the online db must keep it instead of generating its own identity value.
            modelBuilder.Entity<MovieScheduleListReserveSeat>()
                .Property(x => x.Id)
                .ValueGeneratedNever();
        }
    }
}
