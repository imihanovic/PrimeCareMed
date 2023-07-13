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
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Net;

namespace PrimeCareMed.Frontend.Pages.Shift
{
    [Authorize(Roles = "Doctor,Nurse,SysAdministrator")]
    public class ChooseShiftModel : PageModel
    {
        public readonly IShiftService _sessionService;
        public readonly IOfficeService _officeService;
        public readonly IShiftRepository _sessionRepository;
        public readonly IOfficeRepository _officeRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
#nullable enable
        public PaginatedList<TableModel> Tables { get; set; }

#nullable disable
        public List<string> TableModelProperties;

        public IEnumerable<ShiftModel> Shifts { get; set; }

        public int TotalPages { get; set; }

        public ChooseShiftModel(IShiftService shiftService,
            IOfficeService officeService,
            IShiftRepository shiftRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IOfficeRepository officeRepository)
        {
            _sessionService = shiftService;
            _sessionRepository = shiftRepository;
            _officeService = officeService;
            _mapper = mapper;
            _userManager = userManager;
            _officeRepository = officeRepository;
        }

        public IActionResult OnGet()
        {

            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            int pageSize = 9;
            if(currentUserRole == "Doctor")
            {
                Shifts = _sessionService.GetAllShiftsForDoctor(currentUser.Id);
            }
            else if(currentUserRole == "Nurse")
            {
                Shifts = _sessionService.GetAllShiftsForNurse(currentUser.Id);
            }
            else
            {
                return Redirect("../Index");
            }
            var cookie = Request.Cookies["myCookie"];
            Console.WriteLine($"Prije {cookie}");
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
        public ActionResult OnPost(string name, string value)
        {
            //Console.WriteLine($"COOKIE {name}");
            //Console.WriteLine($"COOKIE {value}");
            //var newShiftSession = ;
            HttpContext.Response.Cookies.Append(
                     "myCookie", value,
                     new CookieOptions() { SameSite = SameSiteMode.Lax });
            //var cookie = Request.Cookies["myCookie"];
            //Console.WriteLine($"COOKIE {cookie}");
            return Redirect("../Index");
        }
    }
}
