using AutoMapper;
using PrimeCareMed.Application.Models.Hospital;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.Services
{
    public interface IHospitalService
    {
        Task<HospitalModel> AddAsync(HospitalModelForCreate createHospitalModel);
        List<string> GetHospitalModelFields();
        IEnumerable<HospitalModel> GetAllHospitals();
        IEnumerable<HospitalModel> HospitalSorting(IEnumerable<HospitalModel> hospitals, string sortOrder);
        IEnumerable<HospitalModel> HospitalSearch(IEnumerable<HospitalModel> hospitals, string searchString);
        Hospital EditHospitalAsync(HospitalModelForCreate hospitalModel);
        HospitalModelForCreate GetHospitalById(string Id);
        Task DeleteHospitalAsync(Guid Id);
        IEnumerable<HospitalModel> GetHospitalsByCheckupId(Guid CheckupId);
        HospitalModel GetHospitalModelById(string Id);
    }
}
