using AutoMapper;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.Core.Enums;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentService(IMapper mapper,
            IAppointmentRepository appointmentRepository,
            IShiftRepository shiftRepository,
            IPatientRepository patientRepository
            )
        {
            _mapper = mapper;
            _appointmentRepository = appointmentRepository;
            _shiftRepository = shiftRepository;
            _patientRepository = patientRepository;
        }

        public async Task<AppointmentModel> AddAsync(AppointmentModelForCreate createAppointmentModel)
        {
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
            var appointmentsFromDB = _appointmentRepository.GetAllAppointmentsForDoctorAsync(doctorId);

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
            return appointments.AsEnumerable().OrderByDescending(r=>r.AppointmentDate);
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
            return appointments.AsEnumerable().OrderByDescending(r => r.AppointmentDate);
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
            return appointments.AsEnumerable().OrderByDescending(r => r.AppointmentDate);
        }

        public IEnumerable<AppointmentModel> GetAllAppointmentsInWaitingRoom(string cookie)
        {
            var appointmentsFromDB = _appointmentRepository.GetAllAppointmentsAsync().Result.Where(r => r.Shift.Id.ToString() == cookie).OrderBy(x => x.Status).ToList();
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
            return appointments.AsEnumerable().OrderBy(x => x.Status);
        }
        public IEnumerable<AppointmentModel> GetAllAppointmentsForShift(Guid Id)
        {
            var appointmentsFromDB = _appointmentRepository.GetAllAppointmentsAsync().Result.Where(r => r.Shift.Id == Id).ToList();
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
        public async Task DeleteAppointmentAsync(Guid Id)
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
        public IEnumerable<AppointmentModel> AppointmentSorting(IEnumerable<AppointmentModel> appointments, string sortOrder)
        {
            IEnumerable<AppointmentModel> sortedAppointments = appointments;
            switch (sortOrder)
            {
                case "PatientFirstName":
                    return appointments.OrderBy(s => s.PatientFirstName);
                case "PatientFirstNameDesc":
                    return appointments.OrderByDescending(s => s.PatientFirstName);
                case "PatientLastName":
                    return appointments.OrderBy(s => s.PatientLastName);
                case "PatientLastNameDesc":
                    return appointments.OrderByDescending(s => s.PatientLastName);
                case "PatientMbo":
                    return appointments.OrderBy(s => s.PatientMbo);
                case "PatientMboDesc":
                    return appointments.OrderByDescending(s => s.PatientMbo);
                case "AppointmentDate":
                    return appointments.OrderBy(s => s.AppointmentDate);
                case "AppointmentDateDesc":
                    return appointments.OrderByDescending(s => s.AppointmentDate);
                default:
                    return appointments.OrderByDescending(s => s.AppointmentDate);
            }
        }

        public IEnumerable<AppointmentModel> AppointmentSearch(IEnumerable<AppointmentModel> appointments, string searchString)
        {
            IEnumerable<AppointmentModel> searchedAppointments = appointments;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedAppointments = appointments.Where(s => s.PatientFirstName.ToLower().Contains(searchStrTrim)
                                            || s.PatientLastName.ToLower().Contains(searchStrTrim)
                                            || s.PatientMbo.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedAppointments;
        }

        public IEnumerable<AppointmentModel> AppointmentFilterDate(IEnumerable<AppointmentModel> appointments, string date)
        {
            IEnumerable<AppointmentModel> filteredAppointments = appointments;
            if (!String.IsNullOrEmpty(date))
            {
                var dateTrim = date.ToLower().Trim();
                filteredAppointments = appointments.Where(s => s.AppointmentDate.ToString("yyyy-MM-dd").ToLower().Contains(dateTrim));
            }
            return filteredAppointments;
        }

        public IEnumerable<AppointmentModel> AppointmentFilterStatus(IEnumerable<AppointmentModel> appointments, string status)
        {
            IEnumerable<AppointmentModel> filteredAppointments = appointments;
            if (!String.IsNullOrEmpty(status))
            {
                var statusTrim = status.ToLower().Trim();
                filteredAppointments = appointments.Where(s => s.Status.ToString().ToLower().Contains(statusTrim));
            }
            return filteredAppointments;
        }
    }
}
