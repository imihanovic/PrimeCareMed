using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Entities.Identity;

namespace PrimeCareMed.Core.Entities
{
    public class MedicalReport : BaseEntity
    {
        public string Description { get; set; }
        public DateTime ReportDate { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
