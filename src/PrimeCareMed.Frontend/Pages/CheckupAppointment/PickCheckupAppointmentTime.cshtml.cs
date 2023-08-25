using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Application.Models.HospitalCheckup;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.CheckupAppointment
{
    public class PickCheckupAppointmentTimeModel : PageModel
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICheckupService _checkupService;
        private readonly ICheckupAppointmentService _checkupAppointmentService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IHospitalService _hospitalService;
        public PickCheckupAppointmentTimeModel(IOfficeRepository officeRepository,
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
        [BindProperty]
        public CheckupAppointmentModelForCreate NewCheckupAppointment { get; set; }

#nullable enable
        public List<DateTime>? AvailableSlots { get; set; }
        public string? time { get; set; }
#nullable disable
        public async Task<IActionResult> OnPostAsync(string hospitalId, string checkupId, string checkupDate, string appointmentId)
        {
            var appointment = _appointmentRepository.GetAppointmentByIdAsync(Guid.Parse(appointmentId)).Result;
            var availableSlots = _checkupService.GetAvailableTimeslotsForCheckup(checkupDate, checkupId, hospitalId);
            // VAR GET TIMESLOTS FOR CHECKUP!!!!
            AvailableSlots = availableSlots;
            ViewData["HospitalId"] = hospitalId;
            ViewData["CheckupId"] = checkupId;
            ViewData["CheckupDate"] = checkupDate;
            ViewData["AppointmentId"] = appointmentId;
            var currentdate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd");
            var currentHour = int.Parse(DateTime.Now.ToLocalTime().ToString("HH")) + 2;

            Console.WriteLine($"VRIME {NewCheckupAppointment.Time}");
            Console.WriteLine($"VRIME {time}");
            if(NewCheckupAppointment.Time.ToString("dd/MM/yyyy HH:mm:ss") != "01/01/0001 00:00:00")
            {
                try
                {
                    NewCheckupAppointment.Date = DateTime.Parse(checkupDate).ToUniversalTime();
                    NewCheckupAppointment.AppointmentId = Guid.Parse(appointmentId);
                    NewCheckupAppointment.HospitalId = Guid.Parse(hospitalId);
                    NewCheckupAppointment.CheckupId = Guid.Parse(checkupId);
                    NewCheckupAppointment.CheckupStatus = Core.Enums.CheckupStatus.Active;
                    Console.WriteLine($"PROVJERA{NewCheckupAppointment.Time}");
                    await _checkupAppointmentService.AddCheckupAppointment(NewCheckupAppointment);
                    return RedirectToPage("/Hospital/ViewAllHospitals");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return Page();
        }
    }
}
