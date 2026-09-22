namespace Cinex.API.Models
{
    public class SeatValidationModel
    {
        public int MovieScheduleListID { get; set; }

        public List<int> CinemaSeatIDs { get; set; }
    }
}
