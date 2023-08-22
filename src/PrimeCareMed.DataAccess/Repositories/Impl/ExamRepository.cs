using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class ExamRepository : IExamRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ExamRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<Exam>> GetAllExamsAsync()
        {
            return await _context.Exam.OrderBy(r => r.Id).ToListAsync();
        }
        public async Task<IEnumerable<HospitalExam>> GetAllHospitalExamsAsync(Guid HospitalId)
        {
            return await _context.HospitalExam.Include(r=>r.Exam).Where(r=>r.HospitalId==HospitalId).ToListAsync();
        }
        public async Task<Exam> AddAsync(Exam exam)
        {
            await _context.Exam.AddAsync(exam);
            await _context.SaveChangesAsync();
            return exam;
        }
        public async Task<HospitalExam> AddHospitalExamAsync(HospitalExam hospitalExam)
        {
            await _context.HospitalExam.AddAsync(hospitalExam);
            await _context.SaveChangesAsync();
            return hospitalExam;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Exam.FirstOrDefault(r => r.Id == id);
            _context.Exam.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Exam> UpdateAsync(Exam exam)
        {
            var editItem = await GetExamByIdAsync(exam.Id.ToString());
            editItem.Name = exam.Name;
            editItem.Description = exam.Description;
            editItem.Duration = exam.Duration;
            editItem.Preparation = exam.Preparation;
            await _context.SaveChangesAsync();
                return editItem;
        }
        public async Task<Exam> GetExamByIdAsync(string id)
        {
            return await _context.Exam.FirstOrDefaultAsync(t => t.Id.ToString() == id);
        }
    }
}
