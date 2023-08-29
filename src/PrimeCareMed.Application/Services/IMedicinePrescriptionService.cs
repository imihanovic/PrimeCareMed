using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.Services
{
    public interface IMedicinePrescriptionService
    {
        Task<MedicinePrescriptionModel> AddAsync(MedicinePrescriptionModelForCreate createReportModel, Guid appointmentId);
        List<string> GetMedicinePrescriptionModelFields();
        IEnumerable<MedicinePrescriptionModel> GetMedicinePrescriptionsForAppointment(Guid id);
        MedicinePrescription EditMedicinePrescriptionAsync(MedicinePrescriptionModelForCreate prescriptionModel);
        Task DeleteMedicineAsync(Guid Id);
        IEnumerable<MedicinePrescriptionModel> GetMedicinePrescriptionsForPatient(Guid patientId);
    }
}
