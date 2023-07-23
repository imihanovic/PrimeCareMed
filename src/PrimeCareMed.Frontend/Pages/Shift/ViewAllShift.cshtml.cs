using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;

namespace PrimeCareMed.Frontend.Pages.Shift
{
    [Authorize(Roles = "Administrator,SysAdministrator")]
    public class ViewAllShiftsModel : PageModel
    {
        
        
        public readonly IShiftService _shiftService;
        public readonly IOfficeService _officeService;
        public readonly IShiftRepository _shiftRepository;
        public readonly IOfficeRepository _officeRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
#nullable enable
        public PaginatedList<TableModel> Tables { get; set; }

#nullable disable
        public List<string> TableModelProperties;

        [FromRoute]
        public Guid Id { get; set; }

        public IEnumerable<ShiftModel> Shifts { get; set; }
        public OfficeModel OfficeModel { get; set; }

        public int TotalPages { get; set; }

        public ViewAllShiftsModel(IShiftService shiftService,
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

        public IActionResult OnGet()
        {

            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            int pageSize = 9;

            var office = _officeRepository.GetOfficeByIdAsync(Id.ToString()).Result;

            OfficeModel = _mapper.Map<OfficeModel>(office);

            var shifts = _shiftService.GetAllShiftsForOffice(Id);
            Shifts = shifts.ToList();

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
