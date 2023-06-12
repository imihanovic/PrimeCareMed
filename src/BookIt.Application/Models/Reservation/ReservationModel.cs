namespace BookIt.Application.Models.Reservation
{
    public class ReservationModel : BaseResponseModel
    {
        public string NumberOfPersons { get; set; }

        public string Customer { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Status { get; set; }

        public string ReservationDetails { get; set; }

        public string TableArea { get; set; }

        public string SmokingArea { get; set; }
    }
}
