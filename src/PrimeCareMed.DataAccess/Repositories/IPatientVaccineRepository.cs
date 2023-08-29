using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IPatientVaccineRepository
    {
        Task<IEnumerable<PatientsVaccine>> GetAllPatientsVaccinesForAppointmentAsync(Guid id);
        Task<PatientsVaccine> AddAsync(PatientsVaccine patientsVaccine);
        Task DeleteAsync(Guid id);
        Task<PatientsVaccine> UpdateAsync(PatientsVaccine patientsVaccine);
        Task<PatientsVaccine> GetPatientsVaccineByIdAsync(Guid id);
        bool CheckPatientsVaccinesForAppointmentAsync(Guid id);
        Task<IEnumerable<PatientsVaccine>> GetAllPatientsVaccinesForPatientAsync(Guid id);
    }
}
