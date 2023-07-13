using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Persistence;

namespace PrimeCareMed.DataAccess.Repositories.Impl
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MedicineRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<Medicine>> GetAllMedicinesAsync()
        {
            return await _context.Medicines.OrderBy(r => r.Name).ToListAsync();
        }
        public async Task<Medicine> AddAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            await _context.SaveChangesAsync();
            return medicine;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Medicines.FirstOrDefault(r => r.Id == id);
            _context.Medicines.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Medicine> UpdateAsync(Medicine medicine)
        {
            var editItem = await GetMedicineByIdAsync(medicine.Id);
            editItem.Name = medicine.Name;
            editItem.Description = medicine.Description;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<Medicine> GetMedicineByIdAsync(Guid id)
        {
            return await _context.Medicines.FirstOrDefaultAsync(t => t.Id == id);
        }
    }

}
