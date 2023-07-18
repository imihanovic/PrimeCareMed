namespace PrimeCareMed.Application.Models.PatientVaccine
{
    public class PatientVaccineModel : BaseResponseModel
    {
        public string MedicineName { get; set; }
        public DateTime VaccineDate { get; set; }
        public string Description { get; set; }
    }
}
