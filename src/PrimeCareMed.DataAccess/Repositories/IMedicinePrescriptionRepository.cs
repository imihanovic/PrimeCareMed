using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IMedicinePrescriptionRepository
    {
        Task<IEnumerable<MedicinePrescription>> GetAllMedicalPrecriptionsForAppointmentAsync(Guid Id);
        Task<MedicinePrescription> AddAsync(MedicinePrescription prescription);
        Task DeleteAsync(Guid id);
        Task<MedicinePrescription> UpdateAsync(MedicinePrescription prescription);
        Task<MedicinePrescription> GetMedicalReportByIdAsync(Guid id);
        bool CheckMedicinePrescriptionForAppointmentAsync(Guid id);
        Task<IEnumerable<MedicinePrescription>> GetAllMedicalPrecriptionsForPatientAsync(Guid Id);
    }
}
