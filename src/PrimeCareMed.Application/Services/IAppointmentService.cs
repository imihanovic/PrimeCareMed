using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentModel> AddAsync(AppointmentModelForCreate createAppointmentModel);
        List<string> GetAppointmentModelFields();
        IEnumerable<AppointmentModel> GetAllAppointments(string Id);
        Appointment EditAppointmentAsync(AppointmentModelForCreate appointmentModel);
        Task DeleteAppointmentAsync(Guid Id);
        IEnumerable<PatientModel> GetAllPatientsNotInWaitingRoom(IEnumerable<PatientModel> patientModels, string shiftId);
        IEnumerable<AppointmentModel> GetAllAppointmentsForDoctor(string Id);
        AppointmentDetailsModel GetAppointmentDetailsById(Guid Id);
        IEnumerable<AppointmentModel> GetAllAppointments();
        IEnumerable<AppointmentModel> GetAllAppointmentsInWaitingRoom(string cookie);
        IEnumerable<AppointmentModel> GetAllAppointmentsForShift(Guid Id);
    }
}
