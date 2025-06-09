using Cinex.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Schema;

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
        internal virtual DbSet<CinemaSeat> CinemaSeats { get; set; }
        internal virtual DbSet<Configuration> Configurations { get; set; }
        internal virtual DbSet<Ewallet> Ewallets { get; set; }
        internal virtual DbSet<Movie> Movies { get; set; }
        internal virtual DbSet<MovieSchedule> MovieSchedules { get; set; }
        internal virtual DbSet<MovieScheduleList> MovieScheduleLists { get; set; }
        internal virtual DbSet<MovieScheduleListPatron> MovieScheduleListPatrons { get; set; }
        internal virtual DbSet<MovieScheduleListReserveSeat> MovieScheduleListReserveSeats { get; set; }
        internal virtual DbSet<Mtrcb> Mtrcbs { get; set; }
        internal virtual DbSet<Patron> Patrons { get; set; }
        internal virtual DbSet<Session> Sessions { get; set; }
        internal virtual DbSet<SessionEwallet> SessionEwallets { get; set; }
        internal virtual DbSet<SoundSystem> SoundSystems { get; set; }
        internal virtual DbSet<SystemCode> SystemCodes { get; set; }
        internal virtual DbSet<SystemModule> SystemModules { get; set; }
        internal virtual DbSet<Ticket> Tickets { get; set; }
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

                t.HasMany(x => x.MovieSchedules)
                    .WithOne(x => x.Cinema)
                    .HasForeignKey(x => x.CinemaId)
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

            modelBuilder.Entity<CinemaSeat>(t =>
            {
                t.HasMany(x => x.MovieScheduleListReserveSeats)
                    .WithOne(x => x.CinemaSeat)
                    .HasForeignKey(x => x.CinemaSeatId)
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

                t.HasMany(x => x.MovieScheduleListPatrons)
                    .WithOne(x => x.Patron)
                    .HasForeignKey(x => x.PatronId)
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

            modelBuilder.Entity<Ewallet>(t =>
            {
                t.HasMany(x => x.SessionEwallets)
                    .WithOne(x => x.Ewallet)
                    .HasForeignKey(x => x.EwalletId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<SessionEwallet>(t =>
            {
                t.HasOne(x => x.Session)
                    .WithMany(x => x.SessionEwallets)
                    .HasForeignKey(x => x.SessionId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.Ewallet)
                    .WithMany(x => x.SessionEwallets)
                    .HasForeignKey(x => x.EwalletId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<Session>(t =>
            {
                t.HasKey(x => x.Id);

                t.HasMany(x => x.Tickets)
                    .WithOne(x => x.Session)
                    .HasForeignKey(x => x.SessionId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.SessionEwallets)
                    .WithOne(x => x.Session)
                    .HasForeignKey(x => x.SessionId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<Ticket>(t =>
            {
                t.HasOne(x => x.Session)
                    .WithMany(x => x.Tickets)
                    .HasForeignKey(x => x.SessionId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.MovieScheduleListReserveSeats)
                    .WithOne(x => x.Ticket)
                    .HasForeignKey(x => x.TicketId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.User)
                    .WithMany(x => x.Tickets)
                    .HasForeignKey(x => x.UserId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<Movie>(t =>
            {
                t.HasMany(x => x.MovieSchedules)
                    .WithOne(x => x.Movie)
                    .HasForeignKey(x => x.MovieId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.Mtrcb)
                    .WithMany(x => x.Movies)
                    .HasForeignKey(x => x.RatingId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<MovieSchedule>(t =>
            {
                t.HasOne(x => x.Movie)
                    .WithMany(x => x.MovieSchedules)
                    .HasForeignKey(x => x.MovieId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.MovieScheduleLists)
                    .WithOne(x => x.MovieSchedule)
                    .HasForeignKey(x => x.MoviesScheduleId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<MovieScheduleList>(t =>
            {
                t.HasOne(x => x.MovieSchedule)
                    .WithMany(x => x.MovieScheduleLists)
                    .HasForeignKey(x => x.MoviesScheduleId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.MovieScheduleListReserveSeats)
                    .WithOne(x => x.MovieScheduleList)
                    .HasForeignKey(x => x.MovieScheduleListId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<MovieScheduleListReserveSeat>(t =>
            {
                t.HasOne(x => x.CinemaSeat)
                    .WithMany(x => x.MovieScheduleListReserveSeats)
                    .HasForeignKey(x => x.CinemaSeatId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.MovieScheduleList)
                    .WithMany(x => x.MovieScheduleListReserveSeats)
                    .HasForeignKey(x => x.MovieScheduleListId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.Ticket)
                    .WithMany(x => x.MovieScheduleListReserveSeats)
                    .HasForeignKey(x => x.TicketId)
                    .HasPrincipalKey(x => x.Id);

                t.HasOne(x => x.MovieScheduleListPatron)
                    .WithMany(x => x.MovieScheduleListReserveSeats)
                    .HasForeignKey(x => x.PatronId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<MovieScheduleListPatron>(t =>
            {
                t.HasOne(x => x.Patron)
                    .WithMany(x => x.MovieScheduleListPatrons)
                    .HasForeignKey(x => x.PatronId)
                    .HasPrincipalKey(x => x.Id);

                t.HasMany(x => x.MovieScheduleListReserveSeats)
                    .WithOne(x => x.MovieScheduleListPatron)
                    .HasForeignKey(x => x.PatronId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<Mtrcb>(t =>
            {
                t.HasMany(x => x.Movies)
                    .WithOne(x => x.Mtrcb)
                    .HasForeignKey(x => x.RatingId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<User>(t =>
            {
                t.HasMany(x => x.Tickets)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId)
                    .HasPrincipalKey(x => x.Id);
            });
        }
    }
}
