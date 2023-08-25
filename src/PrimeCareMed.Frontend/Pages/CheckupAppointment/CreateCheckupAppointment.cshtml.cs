using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Application.Models.HospitalCheckup;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.CheckupAppointment
{
    public class CreateCheckupAppointmentModel : PageModel
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICheckupService _checkupService;
        private readonly ICheckupAppointmentService _checkupAppointmentService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IHospitalService _hospitalService;
        public CreateCheckupAppointmentModel(IOfficeRepository officeRepository,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            ICheckupService checkupService,
            ICheckupAppointmentService checkupAppointmentService,
            IAppointmentRepository appointmentRepository,
            IHospitalService hospitalService)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _checkupService = checkupService;
            _checkupAppointmentService = checkupAppointmentService;
            _appointmentRepository = appointmentRepository;
            _hospitalService = hospitalService;
        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public CheckupAppointmentModelForCreate NewCheckupAppointment { get; set; }
#nullable enable
        public IEnumerable<HospitalCheckupModel>? Checkups { get; set; }
        public Guid CheckupId { get; set; }
#nullable disable

        public void OnGet()
        {
            var appointment = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
            Checkups = _checkupService.GetAvailableHospitalCheckupModelsForPatient(appointment.Patient.Id);
        }
    }
}
