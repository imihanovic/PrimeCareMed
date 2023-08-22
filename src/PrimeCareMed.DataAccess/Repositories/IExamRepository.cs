using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IExamRepository
    {
        Task<IEnumerable<Exam>> GetAllExamsAsync();
        Task<Exam> AddAsync(Exam exam);
        Task DeleteAsync(Guid id);
        Task<Exam> UpdateAsync(Exam exam);
        Task<Exam> GetExamByIdAsync(string id);
        Task<IEnumerable<HospitalExam>> GetAllHospitalExamsAsync(Guid HospitalId);
        Task<HospitalExam> AddHospitalExamAsync(HospitalExam hospitalExam);
    }
}
