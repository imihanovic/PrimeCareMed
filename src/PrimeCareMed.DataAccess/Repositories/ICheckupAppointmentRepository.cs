using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Persistence;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface ICheckupAppointmentRepository
    {
        Task<IEnumerable<CheckupAppointment>> GetAllCheckupAppointmentsAsync(Guid CheckupId, Guid HospitalId);
        Task<CheckupAppointment> AddAsync(CheckupAppointment checkupAppointment);
        Task<IEnumerable<CheckupAppointment>> GetAllCheckupAppointmentsForPatientAsync(Guid PatientId);
        Task<HospitalCheckup> AddHospitalCheckupAsync(HospitalCheckup hospitalCheckup);
        Task DeleteCheckupAppointmentAsync(Guid id);
        Task<CheckupAppointment> UpdateAsync(CheckupAppointment checkupAppointment);
        Task<CheckupAppointment> GetCheckupAppointmentByIdAsync(string id);

    }
}
