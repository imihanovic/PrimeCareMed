using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Application.Services.Impl;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;

namespace PrimeCareMed.Frontend.Pages.Appointment
{
    [Authorize(Roles = "Doctor, SysAdministrator")]
    public class EditAppointmentDetailsModel : PageModel
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public EditAppointmentDetailsModel(IAppointmentRepository appointmentRepository,
            IAppointmentService appointmentService,
            IUserRepository userRepository,
            IUserService userService,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            _userService = userService;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public AppointmentModelForCreate EditAppointment { get; set; }

        public void OnGet()
        {
            var appointment = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
            EditAppointment = _mapper.Map<AppointmentModelForCreate>(appointment);
        }
        public IActionResult OnPost()
        {
            //if (!ModelState.IsValid) { Console.WriteLine("MODELSTATE"); return Page(); }
            EditAppointment.Id = Id.ToString();
            try
            {
                _appointmentService.EditAppointmentAsync(EditAppointment);
                //return RedirectToPage("ViewAppointmentDetails", new {id=Id});
                return RedirectToPage("WaitingRoom");
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION");
                Console.WriteLine(ex.Message);
                return Page();
            }
        }

        public IActionResult OnPostDelete()
        {
            _appointmentService.DeleteAppointmentAsync(Id);

            return RedirectToPage("ViewAllPatients");
        }
    }
}
