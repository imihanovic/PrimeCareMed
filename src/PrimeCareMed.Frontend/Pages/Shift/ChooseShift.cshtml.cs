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
using Newtonsoft.Json.Linq;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Frontend.Pages.Shift
{
    [Authorize(Roles = "Doctor,Nurse,SysAdministrator")]
    public class ChooseShiftModel : PageModel
    {
        public readonly IShiftService _shiftService;
        public readonly IOfficeService _officeService;
        public readonly IShiftRepository _shiftRepository;
        public readonly IOfficeRepository _officeRepository;
        public readonly ISessionRepository _sessionRepository;
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
            IOfficeRepository officeRepository,
            ISessionRepository sessionRepository)
        {
            _shiftService = shiftService;
            _shiftRepository = shiftRepository;
            _officeService = officeService;
            _mapper = mapper;
            _userManager = userManager;
            _officeRepository = officeRepository;
            _sessionRepository = sessionRepository;
        }

        public IActionResult OnGet()
        {

            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            var cookie = Request.Cookies["shiftCookie"];
            Console.WriteLine($"Prije {cookie}");
            int pageSize = 9;
            var doctorSession = _sessionRepository.CheckIfOpenSessionExistsForDoctor(currentUser.Id);
            var nurseSession = _sessionRepository.CheckIfOpenSessionExistsForNurse(currentUser.Id);

            // PROVJERIT MED SESTRU I DOKTORA U TRENUTNOM COOKIJU!!
            if (currentUserRole == "Doctor")
            {
                if(doctorSession is null)
                {
                    Shifts = _shiftService.GetAllAvailableShifts(Shifts, currentUser.Id, currentUserRole);
                }
                else
                {
                    HttpContext.Response.Cookies.Append(
                     "sessionCookie", doctorSession.Id.ToString(),
                     new CookieOptions() { SameSite = SameSiteMode.Lax });
                    HttpContext.Response.Cookies.Append(
                    "shiftCookie", doctorSession.Shift.Office.Name + " " + doctorSession.Shift.Office.City,
                    new CookieOptions() { SameSite = SameSiteMode.Lax });
                    HttpContext.Response.Cookies.Append(
                     "shiftCookieDetails", doctorSession.Shift.Nurse.FirstName + " " + doctorSession.Shift.Nurse.LastName,
                     new CookieOptions() { SameSite = SameSiteMode.Lax });
                    return Redirect("../Index");
                }
            }
            else if(currentUserRole == "Nurse")
            {
                if (nurseSession is null)
                {
                    Shifts = _shiftService.GetAllAvailableShifts(Shifts, currentUser.Id, currentUserRole);
                }
                else
                {
                    HttpContext.Response.Cookies.Append(
                     "sessionCookie", nurseSession.Id.ToString(),
                     new CookieOptions() { SameSite = SameSiteMode.Lax });

                    HttpContext.Response.Cookies.Append(
                     "shiftCookie", nurseSession.Shift.Office.Name,
                     new CookieOptions() { SameSite = SameSiteMode.Lax });
                    HttpContext.Response.Cookies.Append(
                     "shiftCookieDetails", nurseSession.Shift.Doctor.FirstName + " " + nurseSession.Shift.Doctor.LastName,
                     new CookieOptions() { SameSite = SameSiteMode.Lax });
                    return Redirect("../Index");
                }   
            }
            else
            {
                return Redirect("../Index");
            }
            // AKO JE DOCTOR IL MED SESTRA I NEMA COOKIJA
            
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
            var shift = _shiftRepository.GetShiftByIdAsync(value).Result;
            var session = new Session();
            session.Shift = shift;
            session.ShiftStartTime = DateTime.UtcNow.ToUniversalTime();

            var newSession = _sessionRepository.AddAsync(session).Result;
            HttpContext.Response.Cookies.Append(
                     "sessionCookie", newSession.Id.ToString(),
                     new CookieOptions() { SameSite = SameSiteMode.Lax });

            if (User.IsInRole("Doctor"))
            {
                HttpContext.Response.Cookies.Append(
                "shiftCookie", shift.Office.Name + " " + shift.Office.City,
                new CookieOptions() { SameSite = SameSiteMode.Lax });
                HttpContext.Response.Cookies.Append(
                 "shiftCookieDetails", shift.Nurse.FirstName + " " + shift.Nurse.LastName,
                 new CookieOptions() { SameSite = SameSiteMode.Lax });
            }
            else if (User.IsInRole("Nurse"))
            {
                HttpContext.Response.Cookies.Append(
                "shiftCookie", shift.Office.Name + " " + shift.Office.City,
                new CookieOptions() { SameSite = SameSiteMode.Lax });
                HttpContext.Response.Cookies.Append(
                 "shiftCookieDetails", shift.Doctor.FirstName + " " + shift.Doctor.LastName,
                 new CookieOptions() { SameSite = SameSiteMode.Lax });
            }
            return Redirect("../Index");
        }
    }
}
