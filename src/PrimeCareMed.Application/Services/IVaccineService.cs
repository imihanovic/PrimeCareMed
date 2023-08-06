using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.Services
{
    public interface IVaccineService
    {
        Task<VaccineModel> AddAsync(VaccineModelForCreate createVaccineModel);
        List<string> GetVaccineModelFields();
        IEnumerable<VaccineModel> GetAllVaccines();
        Vaccine EditVaccineAsync(VaccineModelForCreate vaccineModel);
        Task DeleteTableAsync(Guid Id);
        IEnumerable<VaccineModel> VaccineSearch(IEnumerable<VaccineModel> patients, string searchString);
        IEnumerable<VaccineModel> VaccineSorting(IEnumerable<VaccineModel> vaccines, string sortOrder);
    }
}
