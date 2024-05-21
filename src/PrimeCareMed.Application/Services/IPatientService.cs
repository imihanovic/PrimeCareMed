using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.Services
{
    public interface IPatientService
    {
        Task<PatientModel> AddAsync(PatientModelForCreate createPatientModel);
        List<string> GetPatientModelFields();
        IEnumerable<PatientModel> GetAllPatients(string doctorId);
        Patient EditPatientAsync(PatientModelForCreate patientModel);
        Task DeletePatientAsync(Guid Id);
        IEnumerable<PatientModel> GetAllAvailablePatients(string shiftId);
        IEnumerable<PatientModel> PatientFilter(IEnumerable<PatientModel> patients, string role);
        IEnumerable<PatientModel> PatientSearch(IEnumerable<PatientModel> patients, string searchString);
        IEnumerable<PatientModel> PatientSorting(IEnumerable<PatientModel> patients, string sortOrder);
    }
}
