using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Services
{
    public interface IShiftService
    {
        Task<ShiftModel> AddAsync(ShiftModelForCreate createShiftModel);
        List<string> GetShiftModelFields();
        IEnumerable<ShiftModel> GetAllShiftsForOffice(Guid Id);
        Task DeleteMedicineAsync(Guid Id);
        IEnumerable<ShiftModel> GetAllShiftsForDoctor(string Id);
        IEnumerable<ShiftModel> GetAllShiftsForNurse(string Id);
        IEnumerable<ShiftModel> GetShiftsEnumerable(IEnumerable<Shift> shiftsFromDB);
        Task<bool> CheckIfShiftExists(string officeId, string nurseId, string doctorId);
        IEnumerable<ShiftModel> GetAllShifts();
        ShiftModel GetShiftById(string Id);
    }
}
