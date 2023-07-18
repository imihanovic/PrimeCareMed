using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IMedicalReportRepository
    {
        Task<IEnumerable<MedicalReport>> GetAllMedicalReportsAsync();
        Task<MedicalReport> AddAsync(MedicalReport report);
        Task DeleteAsync(Guid id);
        Task<MedicalReport> UpdateAsync(MedicalReport report);
        Task<MedicalReport> GetMedicalReportByIdAsync(Guid id);
        Task<MedicalReport> GetReportByAppointmentIdAsync(Guid id);
        bool CheckIfReportForAppointmentExists(Guid id);
    }
}
