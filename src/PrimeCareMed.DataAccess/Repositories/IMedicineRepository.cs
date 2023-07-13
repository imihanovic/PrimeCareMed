using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IMedicineRepository
    {
        Task<IEnumerable<Medicine>> GetAllMedicinesAsync();
        Task DeleteAsync(Guid id);
        Task<Medicine> AddAsync(Medicine medicine);
        Task<Medicine> UpdateAsync(Medicine medicine);
        Task<Medicine> GetMedicineByIdAsync(Guid id);

    }
}
