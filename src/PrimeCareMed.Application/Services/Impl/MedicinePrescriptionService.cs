using AutoMapper;
using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class MedicinePrescriptionService : IMedicinePrescriptionService
    {
        private readonly IMapper _mapper;
        private readonly IMedicinePrescriptionRepository _medicinePrescriptionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public MedicinePrescriptionService(IMapper mapper,
            IMedicinePrescriptionRepository medicinePrescriptionRepository,
            IUserRepository userRepository,
            IAppointmentRepository appointmentRepository
            )
        {
            _mapper = mapper;
            _medicinePrescriptionRepository = medicinePrescriptionRepository;
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<MedicinePrescriptionModel> AddAsync(MedicinePrescriptionModelForCreate createReportModel, Guid appointmentId)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<MedicinePrescriptionModelForCreate, MedicinePrescription>();

            });
            var prescription = config.CreateMapper().Map<MedicinePrescription>(createReportModel);
            prescription.DatePrescribed = DateTime.Now.ToUniversalTime();
            prescription.Appointment = _appointmentRepository.GetAppointmentByIdAsync(appointmentId).Result;
            await _medicinePrescriptionRepository.AddAsync(prescription);
            return _mapper.Map<MedicinePrescriptionModel>(prescription);
        }

        public List<string> GetMedicinePrescriptionModelFields()
        {
            var medicineDto = new MedicineModel();
            return medicineDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<MedicinePrescriptionModel> GetAllMedicinePrecriptionsForAppointment(Guid Id)
        {
            var medicinesFromDB = _medicinePrescriptionRepository.GetAllMedicalPrecriptionsForAppointmentAsync(Id).Result;

            List<MedicinePrescriptionModel> medicines = new List<MedicinePrescriptionModel>();
            foreach (var medicine in medicinesFromDB)
            {
                var medicineDto = _mapper.Map<MedicinePrescriptionModel>(medicine);
                medicines.Add(medicineDto);

            }
            return medicines.AsEnumerable();
        }
        public MedicinePrescription EditMedicinePrescriptionAsync(MedicinePrescriptionModelForCreate prescriptionModel)
        {
            var prescription = _mapper.Map<MedicinePrescription>(prescriptionModel);
            //prescription.Appointment = _appointmentRepository.GetAppointmentByIdAsync(Guid.Parse(prescriptionModel.AppointmentId)).Result;
            return _medicinePrescriptionRepository.UpdateAsync(prescription).Result;
        }

        public async Task DeleteMedicineAsync(Guid Id)
        {
            await _medicinePrescriptionRepository.DeleteAsync(Id);
        }
    }
}
