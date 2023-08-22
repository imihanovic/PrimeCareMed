using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Enums;

namespace PrimeCareMed.Core.Entities
{
    public class CheckupAppointment : BaseEntity
    {
        public HospitalCheckup HospitalCheckup { get; set; }
        public Appointment Appointment { get; set; }
        public DateTime CheckupDate { get; set; }
        public CheckupStatus CheckupStatus { get; set; }
    }
}
