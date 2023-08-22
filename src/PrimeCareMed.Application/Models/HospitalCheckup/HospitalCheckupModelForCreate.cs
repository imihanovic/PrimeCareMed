using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrimeCareMed.Application.Models.HospitalCheckup
{
    public class HospitalCheckupModelForCreate
    {
        [Required]
        [Display(Name = "HospitalId")]
        public Guid HospitalId { get; set; }

        [Required]
        [Display(Name = "CheckupId")]
        public Guid CheckupId { get; set; }
    }
}
