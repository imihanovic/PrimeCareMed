using AutoMapper;
using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class CheckupAppointmentService : ICheckupAppointmentService
    {
        private readonly IMapper _mapper;
        private readonly ICheckupAppointmentRepository _checkupAppointmentRepository;
        private readonly ICheckupRepository _checkupRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public CheckupAppointmentService(IMapper mapper,
            ICheckupAppointmentRepository checkupAppointmentRepository,
            ICheckupRepository checkupRepository,
            IAppointmentRepository appointmentRepository
            )
        {
            _mapper = mapper;
            _checkupAppointmentRepository = checkupAppointmentRepository;
            _checkupRepository = checkupRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<CheckupAppointment> AddCheckupAppointment(CheckupAppointmentModelForCreate createCheckupAppointmentModel)
        {

            var checkupAppointment = new CheckupAppointment();
            checkupAppointment.HospitalCheckup = _checkupRepository.GetHospitalCheckupByIdsAsync(createCheckupAppointmentModel.HospitalId, createCheckupAppointmentModel.CheckupId).Result;
            checkupAppointment.Appointment = _appointmentRepository.GetAppointmentByIdAsync(createCheckupAppointmentModel.AppointmentId).Result;
            checkupAppointment.CheckupDate = createCheckupAppointmentModel.Date.Add(createCheckupAppointmentModel.Time.TimeOfDay);
            checkupAppointment.CheckupStatus = createCheckupAppointmentModel.CheckupStatus;
            await _checkupAppointmentRepository.AddAsync(checkupAppointment);
            return checkupAppointment;
        }

        public List<string> GetCheckupAppointmentModelFields()
        {
            var checkupAppointmentDto = new CheckupAppointmentModel();
            return checkupAppointmentDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<CheckupAppointmentModel> GetAllCheckupAppointments(Guid HospitalId, Guid CheckupId)
        {
            var checkupAppointmentsFromDatabase = _checkupAppointmentRepository.GetAllCheckupAppointmentsAsync(CheckupId, HospitalId).Result;

            List<CheckupAppointmentModel> checkupAppointments = new List<CheckupAppointmentModel>();
            foreach (var checkupAppointment in checkupAppointmentsFromDatabase)
            {
                var checkupAppointmentDto = _mapper.Map<CheckupAppointmentModel>(checkupAppointment);
                checkupAppointmentDto.HospitalName = checkupAppointment.HospitalCheckup.Hospital.Name;
                checkupAppointmentDto.HospitalAddressCity = checkupAppointment.HospitalCheckup.Hospital.Address + ", " + checkupAppointment.HospitalCheckup.Hospital.City;
                checkupAppointmentDto.CheckupName = checkupAppointment.HospitalCheckup.Checkup.Name;
                checkupAppointmentDto.CheckupDescription = checkupAppointment.HospitalCheckup.Checkup.Description;
                checkupAppointmentDto.CheckupDuration = checkupAppointment.HospitalCheckup.Checkup.Duration;
                checkupAppointmentDto.CheckupPreparation = checkupAppointment.HospitalCheckup.Checkup.Preparation;
                checkupAppointmentDto.CheckupDate = checkupAppointment.CheckupDate;
                checkupAppointmentDto.CheckupStatus = checkupAppointment.CheckupStatus;
                checkupAppointments.Add(checkupAppointmentDto);

            }
            return checkupAppointments.AsEnumerable();
        }
        public IEnumerable<CheckupModel> GetAllAvailableCheckupAppointments(Guid PatientId)
        {
            var patientCheckupAppointmentsFromDB = _checkupAppointmentRepository.GetAllCheckupAppointmentsForPatientAsync(PatientId).Result.Where(r=>r.CheckupStatus.ToString()=="Active").Select(r=>r.HospitalCheckup.CheckupId);
            var allCheckupsFromDB = _checkupRepository.GetAllCheckupsAsync().Result;
            List<CheckupModel> checkupAppointmentsNotActive = new List<CheckupModel>();
            foreach (var checkup in allCheckupsFromDB)
            {
                if (!patientCheckupAppointmentsFromDB.Contains(checkup.Id))
                {
                    var checkupAppointmentDto = _mapper.Map<CheckupModel>(checkup);
                    checkupAppointmentsNotActive.Add(checkupAppointmentDto);
                }
            }
            return checkupAppointmentsNotActive.AsEnumerable();
        }
        public IEnumerable<CheckupAppointmentModel> CheckupAppointmentSorting(IEnumerable<CheckupAppointmentModel> checkupAppointments, string sortOrder)
        {
            switch (sortOrder)
            {
                case "HospitalName":
                    return checkupAppointments.OrderBy(s => s.CheckupName);
                case "HospitalNameDesc":
                    return checkupAppointments.OrderByDescending(s => s.CheckupName);
                case "CheckupName":
                    return checkupAppointments.OrderBy(s => s.CheckupName);
                case "CheckupNameDesc":
                    return checkupAppointments.OrderByDescending(s => s.CheckupName);
                case "CheckupDate":
                    return checkupAppointments.OrderBy(s => s.CheckupDate);
                case "CheckupDateDesc":
                    return checkupAppointments.OrderByDescending(s => s.CheckupDate);
                default:
                    return checkupAppointments.OrderByDescending(s => s.CheckupDate);
            }
        }
        public IEnumerable<CheckupAppointmentModel> GetAllPatientCheckupAppointments(Guid patientId)
        {
            var checkupAppointmentsFromDatabase = _checkupAppointmentRepository.GetAllCheckupAppointmentsForPatientAsync(patientId).Result;

            List<CheckupAppointmentModel> checkupAppointments = new List<CheckupAppointmentModel>();
            foreach (var checkupAppointment in checkupAppointmentsFromDatabase)
            {
                var checkupAppointmentDto = _mapper.Map<CheckupAppointmentModel>(checkupAppointment);
                checkupAppointmentDto.HospitalName = checkupAppointment.HospitalCheckup.Hospital.Name;
                checkupAppointmentDto.HospitalAddressCity = checkupAppointment.HospitalCheckup.Hospital.Address + ", " + checkupAppointment.HospitalCheckup.Hospital.City;
                checkupAppointmentDto.CheckupName = checkupAppointment.HospitalCheckup.Checkup.Name;
                checkupAppointmentDto.CheckupDescription = checkupAppointment.HospitalCheckup.Checkup.Description;
                checkupAppointmentDto.CheckupDuration = checkupAppointment.HospitalCheckup.Checkup.Duration;
                checkupAppointmentDto.CheckupPreparation = checkupAppointment.HospitalCheckup.Checkup.Preparation;
                checkupAppointmentDto.CheckupDate = checkupAppointment.CheckupDate;
                checkupAppointmentDto.CheckupStatus = checkupAppointment.CheckupStatus;
                checkupAppointments.Add(checkupAppointmentDto);
            }
            return checkupAppointments.AsEnumerable();
        }
        public IEnumerable<CheckupAppointmentModel> CheckupAppointmentSearch(IEnumerable<CheckupAppointmentModel> checkupAppointments, string searchString)
        {
            IEnumerable<CheckupAppointmentModel> searchedCheckupAppointments = checkupAppointments;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedCheckupAppointments = checkupAppointments.Where(s => s.CheckupName.ToLower().Contains(searchStrTrim)
                                            || s.HospitalName.ToLower().Contains(searchStrTrim)
                                            || s.HospitalAddressCity.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedCheckupAppointments;
        }
        public CheckupAppointment EditCheckupAppointmentAsync(CheckupAppointmentModelForCreate checkupAppointmentModel)
        {
            Console.WriteLine($"STATUS u servisu{checkupAppointmentModel.CheckupId}");
            Console.WriteLine($"STATUS u servisu{checkupAppointmentModel.CheckupStatus}");
            var checkupAppointment = _mapper.Map<CheckupAppointment>(checkupAppointmentModel);
            Console.WriteLine($"STATUS u servisu{checkupAppointmentModel.CheckupId}");
            Console.WriteLine($"STATUS u servisu{checkupAppointmentModel.CheckupStatus}");
            return _checkupAppointmentRepository.UpdateAsync(checkupAppointment).Result;
        }
        public CheckupAppointmentModelForCreate GetCheckupAppointmentById(string Id)
        {
            var checkupAppointmentFromDB = _checkupAppointmentRepository.GetCheckupAppointmentByIdAsync(Id).Result;
            return _mapper.Map<CheckupAppointmentModelForCreate>(checkupAppointmentFromDB);
        }
        public CheckupAppointmentModel GetCheckupAppointmentDetailsById(string Id)
        {
            var checkupAppointment = _checkupAppointmentRepository.GetCheckupAppointmentByIdAsync(Id).Result;
            var checkupAppointmentDto = _mapper.Map<CheckupAppointmentModel>(checkupAppointment);
            checkupAppointmentDto.HospitalName = checkupAppointment.HospitalCheckup.Hospital.Name;
            checkupAppointmentDto.HospitalAddressCity = checkupAppointment.HospitalCheckup.Hospital.Address + ", " + checkupAppointment.HospitalCheckup.Hospital.City;
            checkupAppointmentDto.CheckupName = checkupAppointment.HospitalCheckup.Checkup.Name;
            checkupAppointmentDto.CheckupDescription = checkupAppointment.HospitalCheckup.Checkup.Description;
            checkupAppointmentDto.CheckupDuration = checkupAppointment.HospitalCheckup.Checkup.Duration;
            checkupAppointmentDto.CheckupPreparation = checkupAppointment.HospitalCheckup.Checkup.Preparation;
            checkupAppointmentDto.CheckupDate = checkupAppointment.CheckupDate;
            checkupAppointmentDto.CheckupStatus = checkupAppointment.CheckupStatus;
            return checkupAppointmentDto;
        }
        public async Task DeleteCheckupAppointmentAsync(Guid Id)
        {
            await _checkupAppointmentRepository.DeleteCheckupAppointmentAsync(Id);
        }
    }
}
