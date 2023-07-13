using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IOfficeRepository
    {
        Task<IEnumerable<GeneralMedicineOffice>> GetAllOfficesAsync();
        Task DeleteAsync(Guid id);
        Task<GeneralMedicineOffice> AddAsync(GeneralMedicineOffice office);
        Task<GeneralMedicineOffice> UpdateAsync(GeneralMedicineOffice office);
        Task<GeneralMedicineOffice> GetOfficeByIdAsync (string id);
    }
}
