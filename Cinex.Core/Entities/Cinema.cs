using Cinex.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Cinex.Core.Entities
{
    [Table("cinema")]
    public partial class Cinema : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("sound_id")]
        public int SoundId { get; set; }

        [Column("capacity")]
        public int? Capacity { get; set; }

        [Column("in_order")]
        public int InOrder { get; set; }
    }

    public partial class Cinema
    {
        public virtual ICollection<CinemaPatron> Patrons { get; set; }

        public virtual ICollection<CinemaPatronDefault> DefaultPatrons { get; set; }

        public virtual SoundSystem SoundSystem { get; set; }

        public string SoundName => SoundSystem?.Name ?? string.Empty;

        public void AddPatrons(List<Patron> patrons)
        {
            if (!(patrons?.Any() ?? false)) return;

            if (!(Patrons?.Any() ?? false)) return;

            var unaccomodatedPatrons = patrons
                .Where(p => !Patrons.Any(t => t.PatronId == p.Id))
                .ToList();

            unaccomodatedPatrons.ForEach(p =>
            {
                var newCinemaPatron = CinemaPatron.New(cinemaId: Id,
                    patronId: p.Id,
                    price: (double)p.OfficialUnitPrice);

                Patrons.Add(newCinemaPatron);
            });
        }

        public void RemovePatrons(List<Patron> patrons)
        {
            if (!(patrons?.Any() ?? false)) return;

            if (!(Patrons?.Any() ?? false)) return;

            var patronIds = patrons.Select(t => t.Id).ToArray();

            Patrons.Where(p => patronIds.Contains(p.PatronId))
                .ToList()
                .ForEach(p =>
                {
                    p.SetDelete();
                });
        }

        public void AddDefaultPatrons(List<Patron> patrons)
        {
            if (!(patrons?.Any() ?? false)) return;

            if (!(DefaultPatrons?.Any() ?? false)) return;

            var unaccomodatedPatrons = patrons
                .Where(p => !DefaultPatrons.Any(t => t.PatronId == p.Id))
                .ToList();

            unaccomodatedPatrons.ForEach(p =>
            {
                var newDefaultCinemaPatron = CinemaPatronDefault.New(cinemaId: Id,
                    patronId: p.Id);

                DefaultPatrons.Add(newDefaultCinemaPatron);
            });
        }

        public void RemoveDefaultPatrons(List<Patron> patrons)
        {
            if (!(patrons?.Any() ?? false)) return;

            if (!(DefaultPatrons?.Any() ?? false)) return;

            var patronIds = patrons.Select(t => t.Id).ToArray();

            DefaultPatrons.Where(p => patronIds.Contains(p.PatronId ?? 0))
                .ToList()
                .ForEach(p =>
                {
                    p.SetDelete();
                });
        }
    }
}
