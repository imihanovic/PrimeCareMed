using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Enums;

namespace PrimeCareMed.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public DateTime AppointmentDate { get; set; }
        public string Cause { get; set; }
        public AppointmentStatus Status { get; set; }
        public Shift Shift { get; set; }
        public Patient Patient { get; set; }

#nullable enable
        public string? MedicalReport { get; set; }
        public ICollection<PatientsVaccine>? PatientsVaccines { get; set; } = new List<PatientsVaccine>();
        public ICollection<MedicinePrescription>? MedicinePrescriptions { get; set; } = new List<MedicinePrescription>();
        public ICollection<CheckupAppointment>? CheckupAppointments { get; set; } = new List<CheckupAppointment>();
#nullable disable
    }
}
