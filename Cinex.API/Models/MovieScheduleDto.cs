using Cinex.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinex.API.Models
{
    public class MovieScheduleDto
    {
        public int Id { get; set; }
        public ICollection<MovieScheduleListDto> MovieScheduleLists { get; set; }

        public int CinemaId { get; set; }
        public int MovieId { get; set; }
        public DateTime Date { get; set; }
        public class MovieScheduleListDto
        {
            public int Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int SeatType { get; set; }
            public int LayTime { get; set; }
            public int Status { get; set; }

            public string Hash { get; set; }

            public ICollection<MovieScheduleListPatronDto> MovieScheduleListPatrons { get; set; }
            public class MovieScheduleListPatronDto
            {
                public int Id { get; set; }
                public int MovieScheduleListId { get; set; }
                public int PatronId { get; set; }
                public decimal? Price { get; set; }
                public int? IsDefault { get; set; }
            }
        }
    }
    
    
}
