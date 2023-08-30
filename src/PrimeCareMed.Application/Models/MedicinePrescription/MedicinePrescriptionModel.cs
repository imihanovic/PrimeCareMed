namespace PrimeCareMed.Application.Models.MedicinePrescription
{
    public class MedicinePrescriptionModel : BaseResponseModel
    {
        public string MedicineName { get; set; }
        public string Description { get; set; }
        public DateTime DatePrescribed { get; set; }
    }
}
