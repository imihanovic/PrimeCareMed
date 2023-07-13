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
    }
}
