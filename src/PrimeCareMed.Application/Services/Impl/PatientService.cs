using AutoMapper;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

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
        public IEnumerable<PatientModel> GetAllAvailablePatients(string shiftId)
        {
            Console.WriteLine($"POSTOJI shiftID u patientService {shiftId}");
            var allPatients = GetAllPatients();
            return _appointmentService.GetAllPatientsNotInWaitingRoom(allPatients, shiftId);
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
        public IEnumerable<PatientModel> PatientSorting(IEnumerable<PatientModel> patients, string sortOrder)
        {
            IEnumerable<PatientModel> sortedPatients = patients;
            switch (sortOrder)
            {
                case "FirstName":
                    return patients.OrderBy(s => s.FirstName);
                case "FirstNameDesc":
                    return patients = patients.OrderByDescending(s => s.FirstName);
                case "LastName":
                    return patients.OrderBy(s => s.LastName);
                case "LastNameDesc":
                    return patients.OrderByDescending(s => s.LastName);
                case "Oib":
                    return patients.OrderBy(s => s.Oib);
                case "OibDesc":
                    return patients.OrderByDescending(s => s.Oib);
                case "Email":
                    return patients.OrderBy(s => s.Email);
                case "EmailDesc":
                    return patients.OrderByDescending(s => s.Email);
                case "Mbo":
                    return patients.OrderBy(s => s.Mbo);
                case "MboDesc":
                    return patients.OrderByDescending(s => s.Mbo);
                default:
                    return patients.OrderBy(s => s.LastName);
            }
        }

        public IEnumerable<PatientModel> PatientSearch(IEnumerable<PatientModel> patients, string searchString)
        {
            IEnumerable<PatientModel> searchedPatients = patients;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedPatients = patients.Where(s => s.LastName.ToLower().Contains(searchStrTrim)
                                            || s.FirstName.ToLower().Contains(searchStrTrim)
                                            || s.Oib.ToLower().Contains(searchStrTrim)
                                            || s.Email.ToLower().Contains(searchStrTrim)
                                            || s.Mbo.ToLower().Contains(searchStrTrim)
                                            || s.PhoneNumber.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedPatients;
        }

        public IEnumerable<PatientModel> PatientFilter(IEnumerable<PatientModel> patients, string gender)
        {
            IEnumerable<PatientModel> filteredPatients = patients;
            if (!String.IsNullOrEmpty(gender))
            {
                var roleTrim = gender.ToLower().Trim();
                filteredPatients = patients.Where(s => s.Gender.ToString().ToLower() == roleTrim);
            }
            return filteredPatients;
        }
    }
}
