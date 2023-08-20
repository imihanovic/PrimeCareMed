using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Application.Services.Impl;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace PrimeCareMed.Frontend.Pages.Shift
{
    [Authorize(Roles = "Administrator,SysAdministrator")]
    public class ViewAllShiftsForOfficeModel : PageModel
    {
        

        public readonly IShiftService _shiftService;
        public readonly IOfficeService _officeService;
        public readonly IShiftRepository _shiftRepository;
        public readonly IOfficeRepository _officeRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
#nullable enable
        public PaginatedList<ShiftModel> Shifts;
#nullable disable
        public List<string> ShiftModelProperties => _shiftService.GetShiftModelFields();

        [FromRoute]
        public Guid Id { get; set; }

        
        public OfficeModel OfficeModel => _officeService.GetAllOffices().FirstOrDefault(r=>r.Id==Id);

        public int TotalPages { get; set; }

        public ViewAllShiftsForOfficeModel(IShiftService shiftService,
            IOfficeService officeService,
            IShiftRepository shiftRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IOfficeRepository officeRepository)
        {
            _shiftService = shiftService;
            _shiftRepository = shiftRepository;
            _officeService = officeService;
            _mapper = mapper;
            _userManager = userManager;
            _officeRepository = officeRepository;
        }

        public IActionResult OnGet(string sort, string currentFilter, string keyword, string dateFilter, int? pageIndex)
        {

            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            if (keyword != null)
            {
                pageIndex = 1;
            }
            else
            {
                keyword = currentFilter;
            }

            ViewData["CurrentFilter"] = keyword;
            int pageSize = 7;

            var shifts = _shiftService.GetAllShiftsForOffice(Id).OrderByDescending(r => r.ShiftStartTime).ToList();
            ViewData["CurrentSort"] = sort;
            shifts = _shiftService.ShiftSorting(shifts, sort).ToList();

            ViewData["Keyword"] = keyword;
            shifts = _shiftService.ShiftSearch(shifts, keyword).ToList();

            ViewData["DateFilter"] = dateFilter;
            shifts = _shiftService.ShiftFilterDate(shifts, dateFilter).ToList();

            Shifts = PaginatedList<ShiftModel>.Create(shifts, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(shifts.Count(), pageSize));

            return Page();
        }
    }
}
