namespace PrimeCareMed.Application.Models.PatientVaccine
{
    public class PatientVaccineModel : BaseResponseModel
    {
        public string VaccineName { get; set; }
        public string Dosage { get; set; }
        public DateTime VaccineDate { get; set; }
    }
}
