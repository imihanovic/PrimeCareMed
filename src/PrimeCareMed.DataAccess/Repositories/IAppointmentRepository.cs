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
        IEnumerable<Appointment> GetAllAppointmentsForOfficeAsync(string Id);
        IEnumerable<Appointment> GetAllAppointmentsForPatientAsync(string Id);
        IEnumerable<Appointment> GetAllAppointmentsForDoctorAsync(string Id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> AddAsync(Appointment appointment);
        Task DeleteAsync(Guid id);
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task<Appointment> GetAppointmentByIdAsync(Guid id);
        Task<Appointment> FinishAppointmentAsync(Appointment appointment);
    }
}
