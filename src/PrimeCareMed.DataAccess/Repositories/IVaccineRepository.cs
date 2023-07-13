using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IVaccineRepository
    {
        Task<IEnumerable<Vaccine>> GetAllVaccinesAsync();
        Task DeleteAsync(Guid id);
        Task<Vaccine> AddAsync(Vaccine vaccine);
        Task<Vaccine> UpdateAsync(Vaccine vaccine);
        Task<Vaccine> GetVaccineByIdAsync(Guid id);
    }
}
