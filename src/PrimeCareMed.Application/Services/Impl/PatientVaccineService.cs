using AutoMapper;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.PatientVaccine;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class PatientVaccineService : IPatientVaccineService
    {
        private readonly IMapper _mapper;
        private readonly IPatientVaccineRepository _patientVaccineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IVaccineRepository _vaccineRepository;

        public PatientVaccineService(IMapper mapper,
            IPatientVaccineRepository patientVaccineRepository,
            IUserRepository userRepository,
            IAppointmentRepository appointmentRepository,
            IVaccineRepository vaccineRepository
            )
        {
            _mapper = mapper;
            _patientVaccineRepository = patientVaccineRepository;
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
            _vaccineRepository = vaccineRepository;
        }

        public async Task<PatientVaccineModel> AddAsync(PatientVaccineModelForCreate createReportModel, Guid appointmentId)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<PatientVaccineModelForCreate, PatientsVaccine>();

            });
            var patientVaccine = config.CreateMapper().Map<PatientsVaccine>(createReportModel);
            patientVaccine.VaccineDate = DateTime.Now.ToUniversalTime();
            patientVaccine.Appointment = _appointmentRepository.GetAppointmentByIdAsync(appointmentId).Result;
            patientVaccine.Vaccine = _vaccineRepository.GetVaccineByIdAsync(Guid.Parse(createReportModel.VaccineId)).Result;
            await _patientVaccineRepository.AddAsync(patientVaccine);
            return _mapper.Map<PatientVaccineModel>(patientVaccine);
        }

        public List<string> GetPatientVaccineModelFields()
        {
            var vaccineDto = new PatientVaccineModel();
            return vaccineDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public PatientsVaccine EditPatientVaccineAsync(PatientVaccineModelForCreate patientVaccineModel)
        {
            var patientVaccine = _mapper.Map<PatientsVaccine>(patientVaccineModel);
            return _patientVaccineRepository.UpdateAsync(patientVaccine).Result;
        }

        public IEnumerable<PatientVaccineModel> GetPatientVaccineForAppointment(Guid id)
        {
            var patientVaccine = _patientVaccineRepository.GetAllPatientsVaccinesForAppointmentAsync(id).Result;
            List<PatientVaccineModel> patientVaccines = new List<PatientVaccineModel>();
            foreach (var vaccine in patientVaccine)
            {
                var vaccineDto = _mapper.Map<PatientVaccineModel>(vaccine);
                vaccineDto.VaccineName = vaccine.Vaccine.Name;
                vaccineDto.Dosage = vaccine.Dosage;
                vaccineDto.VaccineDate = vaccine.VaccineDate;

                patientVaccines.Add(vaccineDto);

            }
            return patientVaccines.AsEnumerable();
        }
        public IEnumerable<PatientVaccineModel> GetPatientVaccineForPatient(Guid id)
        {
            var patientVaccine = _patientVaccineRepository.GetAllPatientsVaccinesForPatientAsync(id).Result;
            List<PatientVaccineModel> patientVaccines = new List<PatientVaccineModel>();
            foreach (var vaccine in patientVaccine)
            {
                var vaccineDto = _mapper.Map<PatientVaccineModel>(vaccine);
                vaccineDto.VaccineName = vaccine.Vaccine.Name;
                vaccineDto.Dosage = vaccine.Dosage;
                vaccineDto.VaccineDate = vaccine.VaccineDate;

                patientVaccines.Add(vaccineDto);

            }
            return patientVaccines.AsEnumerable();
        }

        public async Task DeletePatientVaccineAsync(Guid Id)
        {
            await _patientVaccineRepository.DeleteAsync(Id);
        }
        public IEnumerable<PatientVaccineModel> VaccineSorting(IEnumerable<PatientVaccineModel> vaccines, string sortOrder)
        {
            switch (sortOrder)
            {
                case "VaccineName":
                    return vaccines.OrderBy(s => s.VaccineName);
                case "VaccineNameDesc":
                    return vaccines.OrderByDescending(s => s.VaccineName);
                case "VaccineDate":
                    return vaccines.OrderBy(s => s.VaccineDate);
                case "VaccineDateDesc":
                    return vaccines.OrderByDescending(s => s.VaccineDate);
                default:
                    return vaccines.OrderByDescending(s => s.VaccineDate);
            }
        }

        public IEnumerable<PatientVaccineModel> VaccineSearch(IEnumerable<PatientVaccineModel> vaccines, string searchString)
        {
            IEnumerable<PatientVaccineModel> searchedVaccines = vaccines;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedVaccines = vaccines.Where(s => s.VaccineName.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedVaccines;
        }
    }
}
