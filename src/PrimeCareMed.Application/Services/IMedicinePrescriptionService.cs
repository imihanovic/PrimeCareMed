using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.Services
{
    public interface IMedicinePrescriptionService
    {
        Task<MedicinePrescriptionModel> AddAsync(MedicinePrescriptionModelForCreate createReportModel, Guid appointmentId);
        List<string> GetMedicinePrescriptionModelFields();
        IEnumerable<MedicinePrescriptionModel> GetAllMedicinePrecriptionsForAppointment(Guid Id);
        MedicinePrescription EditMedicinePrescriptionAsync(MedicinePrescriptionModelForCreate prescriptionModel);
        Task DeleteMedicineAsync(Guid Id);
    }
}
