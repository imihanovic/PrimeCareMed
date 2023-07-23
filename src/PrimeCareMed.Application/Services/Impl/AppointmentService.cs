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
        private readonly IShiftRepository _shiftRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentService(IMapper mapper,
            IAppointmentRepository appointmentRepository,
            IUserRepository userRepository,
            IShiftRepository shiftRepository,
            IPatientRepository patientRepository
            )
        {
            _mapper = mapper;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _shiftRepository = shiftRepository;
            _patientRepository = patientRepository;
        }

        public async Task<AppointmentModel> AddAsync(AppointmentModelForCreate createAppointmentModel)
        {
            //var config = new MapperConfiguration(cfg => {

            //    cfg.CreateMap<AppointmentModelForCreate, Appointment>();

            //});
            //var appointment = config.CreateMapper().Map<Appointment>(createAppointmentModel);
            var appointment = new Appointment();
            appointment.AppointmentDate = DateTime.Now.ToUniversalTime().AddHours(2);
            appointment.Cause = createAppointmentModel.Cause;
            appointment.Shift = _shiftRepository.GetShiftByIdAsync(createAppointmentModel.ShiftId).Result;
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
        public IEnumerable<PatientModel> GetAllPatientsNotInWaitingRoom(IEnumerable<PatientModel> patientModels, string shiftId)
        {
            Console.WriteLine($"POSTOJI ULAZ U SERVIS");
            var availablePatients = new List<PatientModel>();
            if (shiftId == null)
            {
                return availablePatients.AsEnumerable();
            }
            var patientAppointments = _appointmentRepository.GetAllAppointmentsAsync().Result.Where(r=>r.Status.ToString() != "Done" && r.Shift.Id.ToString()==shiftId).Select(r => r.Patient.Id.ToString());
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
            
            var shift = _shiftRepository.GetShiftByIdAsync(Id).Result;
            var doctorId = shift.Doctor.Id;
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
            var appointmentsFromDB = _appointmentRepository.GetAllAppointmentsAsync().Result.Where(r => r.Shift.Id.ToString() == cookie && r.Status.ToString()!="Done").ToList();
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
        public IEnumerable<Medicine> GetAppointmentMedicines(Guid Id)
        {
            var appointment = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
            return appointment.MedicinePrescriptions.Select(r=>r.Medicine);
        }
        //public IEnumerable<PatientsVaccine> GetAppointmentVaccines(Guid Id)
        //{
        //    var appointment = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
        //    var b = appointment.PatientsVaccines;
        //}
    }
}
