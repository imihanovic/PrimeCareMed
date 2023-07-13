using PrimeCareMed.Core.Common;

namespace PrimeCareMed.Core.Entities
{
    public class PatientsVaccine : BaseEntity
    {
        public string Dosage { get; set; }
        public DateTime VaccineDate { get; set; }
        public Vaccine Vaccine { get; set; }
        public Appointment Appointment { get; set; }
        
    }
}
