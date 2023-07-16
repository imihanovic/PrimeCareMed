using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Enums;
using PrimeCareMed.DataAccess.Repositories;
using PrimeCareMed.DataAccess.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Services.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentService(IMapper mapper,
            IAppointmentRepository appointmentRepository,
            IUserRepository userRepository,
            ISessionRepository sessionRepository,
            IPatientRepository patientRepository
            )
        {
            _mapper = mapper;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _patientRepository = patientRepository;
        }

        public async Task<AppointmentModel> AddAsync(AppointmentModelForCreate createAppointmentModel)
        {
            //var config = new MapperConfiguration(cfg => {

            //    cfg.CreateMap<AppointmentModelForCreate, Appointment>();

            //});
            //var appointment = config.CreateMapper().Map<Appointment>(createAppointmentModel);
            var appointment = new Appointment();
            appointment.AppointmentDate = DateTime.Now.ToUniversalTime();
            appointment.Cause = createAppointmentModel.Cause;
            appointment.Session = _sessionRepository.GetSessionByIdAsync(Guid.Parse(createAppointmentModel.SessionId)).Result;
            appointment.Patient = _patientRepository.GetPatientByIdAsync(Guid.Parse(createAppointmentModel.PatientId)).Result;
            appointment.Status = Enum.Parse<AppointmentStatus>(createAppointmentModel.Status);
            await _appointmentRepository.AddAsync(appointment);
            return _mapper.Map<AppointmentModel>(appointment);
        }

        public List<string> GetAppointmentModelFields()
        {
            var appointmentDto = new AppointmentModel();
            return appointmentDto.GetType().GetProperties().Where(x => x.Name != "Id" && x.Name != "Status").Select(x => x.Name).ToList();
        }
        public IEnumerable<PatientModel> GetAllPatientsNotInWaitingRoom(IEnumerable<PatientModel> patientModels, string sessionId)
        {
            var availablePatients = new List<PatientModel>();
            if (sessionId == null)
            {
                return availablePatients.AsEnumerable();
            }
            var patientAppointments = _appointmentRepository.GetAllAppointmentsAsync().Result.Where(r=>r.Status.ToString() != "Done" && r.Session.Id.ToString()==sessionId).Select(r => r.Patient.Id.ToString());
            foreach (var patientModel in patientModels)
            {
                if (!patientAppointments.Contains(patientModel.Id.ToString()))
                {
                    availablePatients.Add(patientModel);
                }
            }
            return availablePatients.AsEnumerable();
        }
       
        public IEnumerable<AppointmentModel> GetAllAppointmentsForDoctor(string Id)
        {
            
            var session = _sessionRepository.GetSessionByIdAsync(Guid.Parse(Id)).Result;
            var doctorId = session.Shift.Doctor.Id;
            var appointmentsFromDB = _appointmentRepository.GetAllAppointmentsForDoctorAsync(doctorId).Result;

            var appointments = new List<AppointmentModel>();
            foreach (var appointment in appointmentsFromDB)
            {
                var appointmentDto = _mapper.Map<AppointmentModel>(appointment);
                appointmentDto.AppointmentDate = appointment.AppointmentDate;
                appointmentDto.Cause = appointment.Cause;
                appointmentDto.PatientFirstName = appointment.Patient.FirstName;
                appointmentDto.PatientLastName = appointment.Patient.LastName;
                appointmentDto.PatientMbo = appointment.Patient.Mbo;
                appointments.Add(appointmentDto);

            }
            return appointments.AsEnumerable();
        }
        public IEnumerable<AppointmentModel> GetAllAppointments(string Id)
        {
            
            var appointmentsFromDB = _appointmentRepository.GetAllAppointmentsForOfficeAsync(Id).Result;

            var appointments = new List<AppointmentModel>();
            foreach (var appointment in appointmentsFromDB)
            {
                var appointmentDto = _mapper.Map<AppointmentModel>(appointment);
                appointmentDto.AppointmentDate = appointment.AppointmentDate;
                appointmentDto.Cause = appointment.Cause;
                appointmentDto.PatientFirstName = appointment.Patient.FirstName;
                appointmentDto.PatientLastName = appointment.Patient.LastName;
                appointmentDto.PatientMbo = appointment.Patient.Mbo;
                appointments.Add(appointmentDto);

            }
            return appointments.AsEnumerable();
        }
        public IEnumerable<AppointmentModel> GetAllAppointments()
        {
            var appointmentsFromDB = _appointmentRepository.GetAllAppointmentsAsync().Result;

            var appointments = new List<AppointmentModel>();
            foreach (var appointment in appointmentsFromDB)
            {
                var appointmentDto = _mapper.Map<AppointmentModel>(appointment);
                appointmentDto.AppointmentDate = appointment.AppointmentDate;
                appointmentDto.Cause = appointment.Cause;
                appointmentDto.PatientFirstName = appointment.Patient.FirstName;
                appointmentDto.PatientLastName = appointment.Patient.LastName;
                appointmentDto.PatientMbo = appointment.Patient.Mbo;
                appointments.Add(appointmentDto);

            }
            return appointments.AsEnumerable();
        }

        public IEnumerable<AppointmentModel> GetAllAppointmentsInWaitingRoom(string cookie)
        {
            var appointmentsFromDB = _appointmentRepository.GetAllAppointmentsAsync().Result.Where(r => r.Session.Id.ToString() == cookie && r.Status.ToString()!="Done").ToList();
            var appointments = new List<AppointmentModel>();
            foreach (var appointment in appointmentsFromDB)
            {
                var appointmentDto = _mapper.Map<AppointmentModel>(appointment);
                appointmentDto.AppointmentDate = appointment.AppointmentDate;
                appointmentDto.Cause = appointment.Cause;
                appointmentDto.PatientFirstName = appointment.Patient.FirstName;
                appointmentDto.PatientLastName = appointment.Patient.LastName;
                appointmentDto.PatientMbo = appointment.Patient.Mbo;
                appointments.Add(appointmentDto);

            }
            return appointments.AsEnumerable();
        }
        public Appointment EditAppointmentAsync(AppointmentModelForCreate appointmentModel)
        {
            var appointment = _mapper.Map<Appointment>(appointmentModel);
            return _appointmentRepository.UpdateAsync(appointment).Result;
        }

        public async Task DeleteMedicineAsync(Guid Id)
        {
            await _appointmentRepository.DeleteAsync(Id);
        }
        public AppointmentDetailsModel GetAppointmentDetailsById(Guid Id)
        {
            var appointment = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
            return _mapper.Map<AppointmentDetailsModel>(appointment);
        }
    }
}
