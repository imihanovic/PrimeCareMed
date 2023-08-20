using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Models.User;
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
        Task DeleteShiftAsync(Guid Id);
        IEnumerable<ShiftModel> GetAllShiftsForDoctor(string Id);
        IEnumerable<ShiftModel> GetAllShiftsForNurse(string Id);
        IEnumerable<ShiftModel> GetShiftsEnumerable(IEnumerable<Shift> shiftsFromDB);
        bool CheckIfShiftExists(string officeId, string nurseId, string doctorId);

        IEnumerable<ListUsersModel> GetAllAvailableNurses();
        IEnumerable<ListUsersModel> GetAllAvailableDoctors();
        IEnumerable<ShiftModel> GetAllShifts();
        ShiftModel GetShiftById(string Id);

        IEnumerable<ShiftModel> ShiftSorting(IEnumerable<ShiftModel> shifts, string sortOrder);
        IEnumerable<ShiftModel> ShiftSearch(IEnumerable<ShiftModel> shifts, string searchString);
        IEnumerable<ShiftModel> ShiftFilterDate(IEnumerable<ShiftModel> shifts, string date);
    }
}
