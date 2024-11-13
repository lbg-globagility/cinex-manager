using Cinex.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinex.Infrastructure.Data
{
    public class CinexContext : DbContext
    {
        public CinexContext(DbContextOptions options) :
            base(options)
        {
        }

        internal virtual DbSet<AuditTrail> AuditTrails { get; set; }
        internal virtual DbSet<Cinema> Cinemas { get; set; }
        internal virtual DbSet<CinemaPatron> CinemaPatrons { get; set; }
        internal virtual DbSet<CinemaPatronDefault> DefaultPatrons { get; set; }
        internal virtual DbSet<Patron> Patrons { get; set; }
        internal virtual DbSet<SoundSystem> SoundSystems { get; set; }
        internal virtual DbSet<SystemCode> SystemCodes { get; set; }
        internal virtual DbSet<SystemModule> SystemModules { get; set; }
        internal virtual DbSet<TicketPrice> TicketPrices { get; set; }
        internal virtual DbSet<User> Users { get; set; }
        internal virtual DbSet<UserLevel> UserLevels { get; set; }
        internal virtual DbSet<UserLevelRight> UserLevelRights { get; set; }
        internal virtual DbSet<UserRight> UserRights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cinema>(t =>
            {
                t.HasMany(x => x.Patrons)
                    .WithOne(x => x.Cinema)
                    .HasForeignKey(x => x.CinemaId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.DefaultPatrons)
                    .WithOne(x => x.Cinema)
                    .HasForeignKey(x => x.CinemaId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.SoundSystem)
                    .WithMany(x => x.Cinemas)
                    .HasForeignKey(x => x.SoundId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<CinemaPatron>(t =>
            {
                t.HasOne(x => x.Cinema)
                    .WithMany(x => x.Patrons)
                    .HasForeignKey(x => x.CinemaId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.Patron)
                    .WithMany(x => x.Patrons)
                    .HasForeignKey(x => x.PatronId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<CinemaPatronDefault>(t =>
            {
                t.HasOne(x => x.Cinema)
                    .WithMany(x => x.DefaultPatrons)
                    .HasForeignKey(x => x.CinemaId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.Patron)
                    .WithMany(x => x.DefaultPatrons)
                    .HasForeignKey(x => x.PatronId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<Patron>(t =>
            {
                t.HasMany(x => x.Patrons)
                    .WithOne(x => x.Patron)
                    .HasForeignKey(x => x.PatronId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.DefaultPatrons)
                    .WithOne(x => x.Patron)
                    .HasForeignKey(x => x.PatronId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.TicketPrice)
                    .WithMany(x => x.Patrons)
                    .HasForeignKey(x => x.BasePriceId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<SoundSystem>(t =>
            {
                t.HasMany(x => x.Cinemas)
                    .WithOne(x => x.SoundSystem)
                    .HasForeignKey(x => x.SoundId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<TicketPrice>(t =>
            {
                t.HasMany(x => x.Patrons)
                    .WithOne(x => x.TicketPrice)
                    .HasForeignKey(x => x.BasePriceId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<SystemCode>(t =>
            {
                t.HasMany(x => x.Modules)
                    .WithOne(x => x.SystemCode)
                    .HasForeignKey(x => x.SystemCodeId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.Users)
                    .WithOne(x => x.SystemCode)
                    .HasForeignKey(x => x.SystemCodeId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.UserLevels)
                    .WithOne(x => x.SystemCode)
                    .HasForeignKey(x => x.SystemCodeId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.UserLevelRights)
                    .WithOne(x => x.SystemCode)
                    .HasForeignKey(x => x.SystemCodeId)
                    .HasPrincipalKey(x => x.Id);
            });
        }
    }
}
