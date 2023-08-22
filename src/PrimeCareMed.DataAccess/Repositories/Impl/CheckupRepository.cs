using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class CheckupRepository : ICheckupRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CheckupRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<Checkup>> GetAllCheckupsAsync()
        {
            return await _context.Checkup.OrderBy(r => r.Id).ToListAsync();
        }
        public async Task<IEnumerable<HospitalCheckup>> GetAllHospitalCheckupsAsync(Guid HospitalId)
        {
            return await _context.HospitalCheckup.Include(r=>r.Checkup).Where(r=>r.HospitalId==HospitalId).ToListAsync();
        }
        public async Task<Checkup> AddAsync(Checkup checkup)
        {
            await _context.Checkup.AddAsync(checkup);
            await _context.SaveChangesAsync();
            return checkup;
        }
        public async Task<HospitalCheckup> AddHospitalCheckupAsync(HospitalCheckup hospitalCheckup)
        {
            await _context.HospitalCheckup.AddAsync(hospitalCheckup);
            await _context.SaveChangesAsync();
            return hospitalCheckup;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Checkup.FirstOrDefault(r => r.Id == id);
            _context.Checkup.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Checkup> UpdateAsync(Checkup checkup)
        {
            var editItem = await GetCheckupByIdAsync(checkup.Id.ToString());
            editItem.Name = checkup.Name;
            editItem.Description = checkup.Description;
            editItem.Duration = checkup.Duration;
            editItem.Preparation = checkup.Preparation;
            await _context.SaveChangesAsync();
                return editItem;
        }
        public async Task<Checkup> GetCheckupByIdAsync(string id)
        {
            return await _context.Checkup.FirstOrDefaultAsync(t => t.Id.ToString() == id);
        }
    }
}
