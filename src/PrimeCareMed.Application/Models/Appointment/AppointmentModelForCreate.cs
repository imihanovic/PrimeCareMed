using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrimeCareMed.Application.Models.Appointment
{
    public class AppointmentModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Cause")]
        public string Cause { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Shift session")]
        public string ShiftId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Patient")]
        public string PatientId { get; set; }

    }
}
