using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HospitalRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<Hospital>> GetAllHospitalsAsync()
        {
            return await _context.Hospital.OrderBy(r => r.Id).ToListAsync();
        }
        public async Task<Hospital> AddAsync(Hospital hospital)
        {
            await _context.Hospital.AddAsync(hospital);
            await _context.SaveChangesAsync();
            return hospital;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Hospital.FirstOrDefault(r => r.Id == id);
            _context.Hospital.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Hospital> UpdateAsync(Hospital hospital)
        {
            var editItem = await GetHospitalByIdAsync(hospital.Id.ToString());
            editItem.Name = hospital.Name;
            editItem.Address = hospital.Address;
            editItem.City = hospital.City;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<Hospital> GetHospitalByIdAsync(string id)
        {
            return await _context.Hospital.FirstOrDefaultAsync(t => t.Id.ToString() == id);
        }
    }
}
