using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;
using PrimeCareMed.DataAccess.Repositories.Impl;

namespace PrimeCareMed.Application.Services.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentService _appointmentService;

        public PatientService(IMapper mapper,
            IPatientRepository patientRepository,
            IUserRepository userRepository,
            IAppointmentService appointmentService
            )
        {
            _mapper = mapper;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _appointmentService = appointmentService;
        }

        public async Task<PatientModel> AddAsync(PatientModelForCreate createPatientModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<PatientModelForCreate, Patient>();

            });
            var patient = config.CreateMapper().Map<Patient>(createPatientModel);
            await _patientRepository.AddAsync(patient);
            return _mapper.Map<PatientModel>(patient);
        }

        public List<string> GetPatientModelFields()
        {
            var patientDto = new PatientModel();
            return patientDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<PatientModel> GetAllPatients()
        {
            var patientsFromDatabase = _patientRepository.GetAllPatientsAsync().Result;

            List<PatientModel> patients = new List<PatientModel>();
            foreach (var patient in patientsFromDatabase)
            {
                var patientDto = _mapper.Map<PatientModel>(patient);
                patientDto.FirstName = patient.FirstName;
                patientDto.LastName = patient.LastName;
                patientDto.DateOfBirth = patient.DateOfBirth;
                patientDto.Email = patient.Email;
                patientDto.PhoneNumber = patient.PhoneNumber;
                patientDto.Oib = patient.Oib;
                patientDto.Mbo = patient.Mbo;
                patientDto.Gender = patient.Gender;
                patients.Add(patientDto);

            }
            return patients.AsEnumerable();
        }
        public IEnumerable<PatientModel> GetAllAvailablePatients(string sessionId)
        {
            var allPatients = GetAllPatients();
            return _appointmentService.GetAllPatientsNotInWaitingRoom(allPatients, sessionId);
        }
        public Patient EditPatientAsync(PatientModelForCreate patientModel)
        {
            var patient = _mapper.Map<Patient>(patientModel);
            return _patientRepository.UpdateAsync(patient).Result;
        }

        public async Task DeletePatientAsync(Guid Id)
        {
            await _patientRepository.DeleteAsync(Id);
        }
    }
}
