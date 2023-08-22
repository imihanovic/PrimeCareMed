using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Application.Models.HospitalCheckup;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Services
{
    public interface ICheckupService
    {
        Task<CheckupModel> AddAsync(CheckupModelForCreate createCheckupModel);
        List<string> GetCheckupModelFields();
        IEnumerable<CheckupModel> GetAllCheckups();
        IEnumerable<CheckupModel> CheckupSorting(IEnumerable<CheckupModel> checkups, string sortOrder);
        IEnumerable<CheckupModel> CheckupSearch(IEnumerable<CheckupModel> checkups, string searchString);
        Checkup EditCheckupAsync(CheckupModelForCreate checkupModel);
        CheckupModelForCreate GetCheckupById(string Id);
        Task DeleteCheckupAsync(Guid Id);
        IEnumerable<CheckupModel> GetAllHospitalCheckups(Guid HospitalId);
        Task<HospitalCheckup> AddHospitalCheckup(HospitalCheckupModelForCreate createHospitalCheckupModel);
        IEnumerable<CheckupModel> GetAllCheckupsNotInHospital(Guid Id);
    }
}
