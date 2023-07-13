using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.Services
{
    public interface IPatientService
    {
        Task<PatientModel> AddAsync(PatientModelForCreate createPatientModel);
        List<string> GetPatientModelFields();
        IEnumerable<PatientModel> GetAllPatients();
        Patient EditPatientAsync(PatientModelForCreate patientModel);
        Task DeletePatientAsync(Guid Id);
        IEnumerable<PatientModel> GetAllAvailablePatients(string shiftId);
    }
}
