using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrimeCareMed.Application.Models.PatientVaccine
{
    public class PatientVaccineModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string VaccineId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Dosage (mg)")]
        public string Dosage { get; set; }
    }
}
