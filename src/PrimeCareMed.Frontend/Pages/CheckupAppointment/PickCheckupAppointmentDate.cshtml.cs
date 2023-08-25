using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PrimeCareMed.Frontend.Pages.CheckupAppointment
{
    public class PickCheckupAppointmentDateModel : PageModel
    {
        public IActionResult OnPost(string selectCheckup, string appointmentId)
        {

            var splitString = selectCheckup.Split(',');
            var hospitalId = splitString[0];
            var checkupId = splitString[1];
            ViewData["HospitalId"] = hospitalId;
            ViewData["AppointmentId"] = appointmentId;
            ViewData["CheckupId"] = checkupId;
            return Page();
        }
    }
}
