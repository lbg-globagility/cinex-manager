namespace Cinex.API.Models
{
    public class MovieScheduleListDto
    {
        public MovieScheduleDto MovieSchedule { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public MovieScheduleListPatronDto DefaultPatron { get; set; }
        public class MovieScheduleDto
        {
            public DateTime Date { get; set; }
            public CinemaDto Cinema { get; set; }

            public class CinemaDto
            {
                public string Name { get; set; }

                public List<CinemaSeatDto> Seats { get; set; }

                public int maxHeight { get; set; }

                public int maxWidth { get; set; }

                public class CinemaSeatDto
                {
                    public int CinemaId { get; set; }
                    public int x1 { get; set; }
                    public int x2 { get; set; }
                    public int y1 { get; set; }
                    public int y2 { get; set; }
                    public string? ColName { get; set; }
                    public string? Name { get; set; }
                    public string? GroupName { get; set; }
                    public string? SectionName { get; set; }
                    public int ObjectType { get; set; }
                    public bool Selected { get; set; }
                    public string Status { get; set; }
                    public int Id { get; set; }

                    public bool IOccupied { get; set; }
                }
            }
        }
        public class MovieScheduleListPatronDto
        {
            public int MovieScheduleListId { get; set; }
            public int PatronId { get; set; }
            public decimal? Price { get; set; }
            public bool IsDefault { get; set; }

            public int Id { get; set; }
        }
    }
}
