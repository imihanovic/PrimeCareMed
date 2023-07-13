using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IShiftRepository
    {
        Task<IEnumerable<Shift>> GetAllShiftsAsync();
        Task<Shift> AddAsync(Shift shiftSession);
        Task DeleteAsync(Guid id);
        Task<Shift> GetShiftByIdAsync(string id);
        Task<IEnumerable<Shift>> GetAllShiftsForOffice(Guid id);
        Task<IEnumerable<Shift>> GetAllShiftsForDoctor(string id);
        Task<IEnumerable<Shift>> GetAllShiftsForNurse(string id);
    }
}
