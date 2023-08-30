using AutoMapper;
using PrimeCareMed.Application.Models.MedicinePrescription;
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
        private readonly IMedicineRepository _medicineRepository;

        public MedicinePrescriptionService(IMapper mapper,
            IMedicinePrescriptionRepository medicinePrescriptionRepository,
            IUserRepository userRepository,
            IAppointmentRepository appointmentRepository,
            IMedicineRepository medicineRepository
            )
        {
            _mapper = mapper;
            _medicinePrescriptionRepository = medicinePrescriptionRepository;
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
            _medicineRepository = medicineRepository;
        }
        public IEnumerable<MedicinePrescriptionModel> GetMedicinePrescriptionsForAppointment(Guid id)
        {
            var medicinePrescriptionsDB = _medicinePrescriptionRepository.GetAllMedicalPrecriptionsForAppointmentAsync(id).Result;
            List<MedicinePrescriptionModel> medicinePrescriptions = new List<MedicinePrescriptionModel>();
            foreach (var prescription in medicinePrescriptionsDB)
            {
                var prescriptionDto = _mapper.Map<MedicinePrescriptionModel>(prescription);
                prescriptionDto.MedicineName = prescription.Medicine.Name;
                prescriptionDto.Description = prescription.Description;
                prescriptionDto.DatePrescribed = prescription.DatePrescribed;

                medicinePrescriptions.Add(prescriptionDto);

            }
            return medicinePrescriptions.AsEnumerable();
        }
        public IEnumerable<MedicinePrescriptionModel> GetMedicinePrescriptionsForPatient(Guid patientId)
        {
            var medicinePrescriptionsDB = _medicinePrescriptionRepository.GetAllMedicalPrecriptionsForPatientAsync(patientId).Result;
            List<MedicinePrescriptionModel> medicinePrescriptions = new List<MedicinePrescriptionModel>();
            foreach (var prescription in medicinePrescriptionsDB)
            {
                var prescriptionDto = _mapper.Map<MedicinePrescriptionModel>(prescription);
                prescriptionDto.MedicineName = prescription.Medicine.Name;
                prescriptionDto.Description = prescription.Description;
                prescriptionDto.DatePrescribed = prescription.DatePrescribed;

                medicinePrescriptions.Add(prescriptionDto);

            }
            return medicinePrescriptions.AsEnumerable();
        }
        public async Task<MedicinePrescriptionModel> AddAsync(MedicinePrescriptionModelForCreate createReportModel, Guid appointmentId)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<MedicinePrescriptionModelForCreate, MedicinePrescription>();

            });
            var prescription = config.CreateMapper().Map<MedicinePrescription>(createReportModel);
            prescription.DatePrescribed = DateTime.Now.ToUniversalTime();
            prescription.Appointment = _appointmentRepository.GetAppointmentByIdAsync(appointmentId).Result;
            prescription.Medicine = _medicineRepository.GetMedicineByIdAsync(Guid.Parse(createReportModel.MedicineId)).Result;
            await _medicinePrescriptionRepository.AddAsync(prescription);
            return _mapper.Map<MedicinePrescriptionModel>(prescription);
        }

        public List<string> GetMedicinePrescriptionModelFields()
        {
            var medicineDto = new MedicinePrescriptionModel();
            return medicineDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public MedicinePrescription EditMedicinePrescriptionAsync(MedicinePrescriptionModelForCreate prescriptionModel)
        {
            var prescription = _mapper.Map<MedicinePrescription>(prescriptionModel);
            return _medicinePrescriptionRepository.UpdateAsync(prescription).Result;
        }

        public async Task DeleteAsync(Guid Id)
        {
            await _medicinePrescriptionRepository.DeleteAsync(Id);
        }
        public IEnumerable<MedicinePrescriptionModel> MedicinePrescriptionSorting(IEnumerable<MedicinePrescriptionModel> prescriptions, string sortOrder)
        {
            IEnumerable<MedicinePrescriptionModel> sortedPrescriptions = prescriptions;
            switch (sortOrder)
            {
                case "MedicineName":
                    return prescriptions.OrderBy(s => s.MedicineName);
                case "MedicineNameDesc":
                    return prescriptions.OrderByDescending(s => s.MedicineName);
                case "DatePrescribed":
                    return prescriptions.OrderBy(s => s.DatePrescribed);
                case "DatePrescribedDesc":
                    return prescriptions.OrderByDescending(s => s.DatePrescribed);
                default:
                    return prescriptions.OrderByDescending(s => s.DatePrescribed);
            }
        }

        public IEnumerable<MedicinePrescriptionModel> MedicinePrescriptionSearch(IEnumerable<MedicinePrescriptionModel> prescriptions, string searchString)
        {
            IEnumerable<MedicinePrescriptionModel> searchedPrescriptions = prescriptions;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedPrescriptions = prescriptions.Where(s => s.MedicineName.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedPrescriptions;
        }
    }
}
