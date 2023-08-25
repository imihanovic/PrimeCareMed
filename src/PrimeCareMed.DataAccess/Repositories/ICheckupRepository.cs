using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface ICheckupRepository
    {
        Task<IEnumerable<Checkup>> GetAllCheckupsAsync();
        Task<Checkup> AddAsync(Checkup checkup);
        Task DeleteAsync(Guid id);
        Task<Checkup> UpdateAsync(Checkup checkup);
        Task<Checkup> GetCheckupByIdAsync(string id);
        Task<IEnumerable<HospitalCheckup>> GetAllHospitalCheckupsAsync(Guid HospitalId);
        Task<HospitalCheckup> AddHospitalCheckupAsync(HospitalCheckup hospitalCheckup);
        Task<IEnumerable<HospitalCheckup>> GetAllCheckupHospitalsAsync(Guid CheckupId);
        Task<HospitalCheckup> GetHospitalCheckupByIdsAsync(Guid HospitalId, Guid CheckupId);
        Task<IEnumerable<HospitalCheckup>> GetAllHospitalCheckupsAsync();
    }
}
