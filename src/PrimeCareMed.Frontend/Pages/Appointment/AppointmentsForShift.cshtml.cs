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
#nullable enable
        //public PaginatedList<TableModel> Tables { get; set; }

#nullable disable
        public List<string> AppointmentModelProperties => _appointmentService.GetAppointmentModelFields();

        [FromRoute]
        public Guid Id { get; set; }
        public IEnumerable<AppointmentModel> Appointments => _appointmentService.GetAllAppointmentsForShift(Id).OrderByDescending(r => r.AppointmentDate);
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

        public IActionResult OnGet()
        {

            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            //AllTables = tables;

            //ViewData["CurrentSort"] = sort;
            //tables = _tableService.TableSorting(tables, sort);

            //ViewData["AreaFilter"] = areaFilter;
            //tables = _tableService.TableFilterByArea(tables, areaFilter);

            //ViewData["SmokingFilter"] = smokingFilter;
            //tables = _tableService.TableFilterBySmoking(tables, smokingFilter);

            //Tables = PaginatedList<TableModel>.Create(tables, pageIndex ?? 1, pageSize);

            //TotalPages = (int)Math.Ceiling(decimal.Divide(tables.Count(), pageSize));

            return Page();
        }
    }
}
