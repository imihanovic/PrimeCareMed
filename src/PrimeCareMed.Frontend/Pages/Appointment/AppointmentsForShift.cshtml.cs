using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Models.User;

namespace PrimeCareMed.Frontend.Pages.Appointment
{
    [Authorize(Roles = "Administrator,SysAdministrator")]
    public class AppointmentsForShiftModel : PageModel
    {
        public readonly IAppointmentService _appointmentService;
        public readonly IShiftService _shiftService;
        public readonly IAppointmentRepository _appointmentRepository;
        public readonly IShiftRepository _shiftRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public List<string> AppointmentModelProperties { get; set; }

        [FromRoute]
        public Guid Id { get; set; }
        public PaginatedList<AppointmentModel> Appointments { get; set; }
        public ShiftModel ShiftModel => _shiftService.GetAllShifts().FirstOrDefault(r => r.Id == Id);

        public int TotalPages { get; set; }

        public AppointmentsForShiftModel(IAppointmentService appointmentService,
            IShiftService shiftService,
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IShiftRepository shiftRepository)
        {
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
            _shiftService = shiftService;
            _mapper = mapper;
            _userManager = userManager;
            _shiftRepository = shiftRepository;
        }

        public IActionResult OnGet(string sort, string currentFilter, string keyword, string dateFilter, int? pageIndex)

        {
            AppointmentModelProperties = _appointmentService.GetAppointmentModelFields();
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (currentUser is null)
            {
                return Redirect("/Identity/Account/Login");
            }
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            var appointments = new List<AppointmentModel>();
            appointments = _appointmentService.GetAllAppointmentsForShift(Id).OrderByDescending(r => r.AppointmentDate).ToList();
            if (keyword != null)
            {
                pageIndex = 1;
            }
            else
            {
                keyword = currentFilter;
            }
            int pageSize = 7;

            ViewData["CurrentSort"] = sort;
            if (sort != "")
            {
                appointments = _appointmentService.AppointmentSorting(appointments, sort).ToList();
            }

            ViewData["Keyword"] = keyword;
            appointments = _appointmentService.AppointmentSearch(appointments, keyword).ToList();

            ViewData["DateFilter"] = dateFilter;
            appointments = _appointmentService.AppointmentFilterDate(appointments, dateFilter).ToList();

            TotalPages = (int)Math.Ceiling(decimal.Divide(appointments.Count(), pageSize));
            Appointments = PaginatedList<AppointmentModel>.Create(appointments, pageIndex ?? 1, pageSize);
            return Page();
        }
    }
}
