using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.Services
{
    public interface IOfficeService
    {
        Task<OfficeModel> AddAsync(OfficeModelForCreate createOfficeModel);
        List<string> GetOfficeModelFields();
        IEnumerable<OfficeModel> GetAllOffices();
        GeneralMedicineOffice EditOfficeAsync(OfficeModelForCreate officeModel);
        Task DeleteOfficeAsync(Guid Id);
        OfficeModelForCreate GetOfficeById(string Id);
    }
}
