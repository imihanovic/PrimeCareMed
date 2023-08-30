using PrimeCareMed.Core.Enums;

namespace PrimeCareMed.Application.Models.CheckupAppointment
{
    public class CheckupAppointmentModel : BaseResponseModel
    {
        public string HospitalName { get; set; }
        public string HospitalAddressCity { get; set; }
        public string CheckupName { get; set; }
        public string CheckupDescription { get; set; }
        public int CheckupDuration { get; set; }
        public string CheckupPreparation { get; set; }
        public DateTime CheckupDate { get; set; }
        public CheckupStatus CheckupStatus { get; set; }
    }
}
