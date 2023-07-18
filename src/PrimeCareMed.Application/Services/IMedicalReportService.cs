using PrimeCareMed.Application.Models.MedicalReport;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.Services
{
    public interface IMedicalReportService
    {
        Task<MedicalReportModel> AddAsync(MedicalReportModelForCreate createReportModel, Guid appointmentId);
        IEnumerable<MedicineModel> GetAllMedicalPrescriptions();
        MedicalReportModel GetReportForAppointment(Guid Id);
        MedicalReport EditMedicalReportAsync(MedicalReportModelForCreate reportModel);
        Task DeleteMedicineAsync(Guid Id);
    }
}
