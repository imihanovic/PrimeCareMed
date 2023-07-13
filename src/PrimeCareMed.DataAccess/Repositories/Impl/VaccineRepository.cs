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
    public class VaccineRepository : IVaccineRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public VaccineRepository(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }
        public async Task<IEnumerable<Vaccine>> GetAllVaccinesAsync()
        {
            return await _context.Vaccines.OrderBy(r => r.Name).ToListAsync();
        }
        public async Task<Vaccine> AddAsync(Vaccine vaccine)
        {
            await _context.Vaccines.AddAsync(vaccine);
            await _context.SaveChangesAsync();
            return vaccine;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Vaccines.FirstOrDefault(r => r.Id == id);
            _context.Vaccines.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Vaccine> UpdateAsync(Vaccine vaccine)
        {
            var editItem = await GetVaccineByIdAsync(vaccine.Id);
            editItem.Name = vaccine.Name;
            editItem.SideEffects = vaccine.SideEffects;
            await _context.SaveChangesAsync();
            return editItem;
        }
        public async Task<Vaccine> GetVaccineByIdAsync(Guid id)
        {
            return await _context.Vaccines.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
