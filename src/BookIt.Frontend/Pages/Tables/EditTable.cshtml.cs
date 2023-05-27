using AutoMapper;
using BookIt.Application.Models.Table;
using BookIt.Application.Services;
using BookIt.Core.Entities;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace BookIt.Frontend.Pages.Tables
{
    public class EditTableModel : PageModel
    {
        private readonly ITableRepository _tableRepository;
        private readonly ITableService _tableService;
        private readonly IMapper _mapper;

        public EditTableModel(ITableRepository tableRepository,
            ITableService tableService,
            IMapper mapper)
        {
            _tableRepository = tableRepository;
            _tableService = tableService;
            _mapper = mapper;
        }

        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public TableModelForUpdate EditTable { get; set; }

        public Table Table { get; set; }

        public void OnGet()
        {
            var table = _tableRepository.GetTableByIdAsync(Id).Result;
            var restaurantName = _tableRepository.GetRestaurantByTableAsync(Id).Result.RestaurantName;
            ViewData["TableName"] = table.TableName;
            ViewData["RestaurantName"] = restaurantName;
            ViewData["RestaurantId"] = Id;
            EditTable = _mapper.Map<TableModelForUpdate>(table);
        }
        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid) { return Page(); }

            EditTable.Id = Id.ToString();

            var restaurantId = _tableRepository.GetRestaurantByTableAsync(Id).Result.Id.ToString();

            _tableService.EditTableAsync(EditTable);

            return RedirectToPage("ViewAllTables", new { id = restaurantId });
        }

        public IActionResult OnPostDelete()
        {
            var restaurantId = _tableRepository.GetRestaurantByTableAsync(Id).Result.Id.ToString();

            _tableService.DeleteTableAsync(Id);

            return RedirectToPage("ViewAllTables", new { id = restaurantId });
        }
    }
}
