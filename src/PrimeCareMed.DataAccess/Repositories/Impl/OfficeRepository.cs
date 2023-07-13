using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public OfficeRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<GeneralMedicineOffice>> GetAllOfficesAsync()
        {
            return await _context.GeneralMedicineOffices.OrderBy(r => r.Id).ToListAsync();
        }
        public async Task<GeneralMedicineOffice> AddAsync(GeneralMedicineOffice office)
        {
            await _context.GeneralMedicineOffices.AddAsync(office);
            await _context.SaveChangesAsync();
            return office;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.GeneralMedicineOffices.FirstOrDefault(r => r.Id == id);
            _context.GeneralMedicineOffices.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<GeneralMedicineOffice> UpdateAsync(GeneralMedicineOffice office)
        {
            var editItem = await GetOfficeByIdAsync(office.Id.ToString());
            editItem.Address = office.Address;
            editItem.City = office.City;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<GeneralMedicineOffice> GetOfficeByIdAsync(string id)
        {
            return await _context.GeneralMedicineOffices.FirstOrDefaultAsync(t => t.Id.ToString() == id);
        }
    }
}
