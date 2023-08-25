using PrimeCareMed.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrimeCareMed.Application.Models.CheckupAppointment
{
    public class CheckupAppointmentModelForCreate
    {
        [Required]
        [Display(Name = "HospitalId")]
        public Guid HospitalId { get; set; }

        [Required]
        [Display(Name = "CheckupId")]
        public Guid CheckupId { get; set; }

        [Required]
        [Display(Name = "AppointmentId")]
        public Guid AppointmentId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date*")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Time*")]
        public DateTime Time { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Status*")]
        public CheckupStatus CheckupStatus { get; set; }
    }
}
