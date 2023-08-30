namespace PrimeCareMed.Application.Models.Appointment
{
    public class AppointmentDetailsModel : BaseResponseModel
    {
        public string PatientMbo { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime PatientDateOfBirth { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Cause { get; set; }
        public string Conclusion { get; set; }
        public string Status { get; set; }
        public string PatientOib { get; set; }
        public string PatientGender { get; set; }
#nullable enable
        public string? MedicalReport { get; set; }
#nullable disable
    }
}
