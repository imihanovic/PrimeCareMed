using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> AddAsync(Patient patient);
        Task DeleteAsync(Guid id);
        Task<Patient> UpdateAsync(Patient patient);
        Task<Patient> GetPatientByIdAsync(Guid id);
    }
}
