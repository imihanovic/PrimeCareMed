using AutoMapper;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.PatientVaccine;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine($"AJDI VAKCINE SERVIS {createReportModel.VaccineId}");
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
            var medicineDto = new MedicineModel();
            return medicineDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
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

        public async Task DeleteMedicineAsync(Guid Id)
        {
            await _patientVaccineRepository.DeleteAsync(Id);
        }
    }
}
