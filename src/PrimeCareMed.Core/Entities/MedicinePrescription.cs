using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Entities.Identity;

namespace PrimeCareMed.Core.Entities
{
    public class MedicinePrescription : BaseEntity
    {
        public Appointment Appointment { get; set; }
        public Medicine Medicine { get; set; }
        public DateTime DatePrescribed { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}
