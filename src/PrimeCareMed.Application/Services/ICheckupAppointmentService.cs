using AutoMapper;
using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Services
{
    public interface ICheckupAppointmentService
    {
        Task<CheckupAppointment> AddCheckupAppointment(CheckupAppointmentModelForCreate createCheckupAppointmentModel);
        List<string> GetCheckupAppointmentModelFields();
        IEnumerable<CheckupAppointmentModel> GetAllCheckupAppointments(Guid HospitalId, Guid CheckupId);
        IEnumerable<CheckupModel> GetAllAvailableCheckupAppointments(Guid PatientId);
        IEnumerable<CheckupAppointmentModel> CheckupAppointmentSorting(IEnumerable<CheckupAppointmentModel> checkupAppointments, string sortOrder);
        IEnumerable<CheckupAppointmentModel> GetAllPatientCheckupAppointments(Guid patientId);
        IEnumerable<CheckupAppointmentModel> CheckupAppointmentSearch(IEnumerable<CheckupAppointmentModel> checkupAppointments, string searchString);
        CheckupAppointment EditCheckupAppointmentAsync(CheckupAppointmentModelForCreate checkupAppointmentModel);
        CheckupAppointmentModelForCreate GetCheckupAppointmentById(string Id);
        Task DeleteCheckupAppointmentAsync(Guid Id);
        CheckupAppointmentModel GetCheckupAppointmentDetailsById(string Id);

    }
}
