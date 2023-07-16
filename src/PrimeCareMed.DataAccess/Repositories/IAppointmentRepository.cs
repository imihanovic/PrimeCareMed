using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsForOfficeAsync(string Id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsForDoctorAsync(string Id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> AddAsync(Appointment appointment);
        Task DeleteAsync(Guid id);
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task<Appointment> GetAppointmentByIdAsync(Guid id);
    }
}
