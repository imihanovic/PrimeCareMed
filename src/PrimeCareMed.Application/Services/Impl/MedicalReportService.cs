using AutoMapper;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.MedicalReport;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class MedicalReportService : IMedicalReportService
    {
        private readonly IMapper _mapper;
        private readonly IMedicalReportRepository _medicalReportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public MedicalReportService(IMapper mapper,
            IMedicalReportRepository medicalReportRepository,
            IUserRepository userRepository,
            IAppointmentRepository appointmentRepository
            )
        {
            _mapper = mapper;
            _medicalReportRepository = medicalReportRepository;
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<MedicalReportModel> AddAsync(MedicalReportModelForCreate createReportModel, Guid appointmentId)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<MedicalReportModelForCreate, MedicalReport>();

            });
            var report = config.CreateMapper().Map<MedicalReport>(createReportModel);
            report.ReportDate = DateTime.Now.ToUniversalTime();
            report.AppointmentId = appointmentId;
            await _medicalReportRepository.AddAsync(report);
            return _mapper.Map<MedicalReportModel>(report);
        }

        //public List<string> GetMedicineModelFields()
        //{
        //    var medicineDto = new MedicineModel();
        //    return medicineDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        //}
        public IEnumerable<MedicineModel> GetAllMedicalPrescriptions()
        {
            var medicinesFromDatabase = _medicalReportRepository.GetAllMedicalReportsAsync().Result;

            List<MedicineModel> medicines = new List<MedicineModel>();
            foreach (var medicine in medicinesFromDatabase)
            {
                var medicineDto = _mapper.Map<MedicineModel>(medicine);
                medicineDto.Description = medicine.Description;
                medicines.Add(medicineDto);

            }
            return medicines.AsEnumerable();
        }
        //public MedicalReportModel GetReportForAppointment(Guid id)
        //{
        //    var reportFromDB = _medicalReportRepository.GetAllMedicalReportsAsync().Result.FirstOrDefault(r=>r.AppointmentId == id);
        //    if(reportFromDB is not null)
        //    {
        //        return _mapper.Map<MedicalReportModel>(reportFromDB);
        //    }
        //    return null;
        //}
        public MedicalReportModel GetReportForAppointment(Guid Id)
        {
            var report = _medicalReportRepository.GetReportByAppointmentIdAsync(Id).Result;
            Console.WriteLine("SERVIS NAKON REPOZITORIJA");
            return _mapper.Map<MedicalReportModel>(report);
        }
        public MedicalReport EditMedicalReportAsync(MedicalReportModelForCreate reportModel)
        {
            var report = _mapper.Map<MedicalReport>(reportModel);
            //report.Appointment = _appointmentRepository.GetAppointmentByIdAsync(Guid.Parse(reportModel.AppointmentId)).Result;
            return _medicalReportRepository.UpdateAsync(report).Result;
        }

        public async Task DeleteMedicineAsync(Guid Id)
        {
            await _medicalReportRepository.DeleteAsync(Id);
        }
    }
}
