namespace PrimeCareMed.Application.Models.Appointment
{
    public class AppointmentModel : BaseResponseModel
    {
        public string PatientMbo { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Cause { get; set; }
        public string Status { get; set; }
    }
}
