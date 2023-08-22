using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrimeCareMed.Application.Models.HospitalExam
{
    public class HospitalExamModelForCreate
    {
        [Required]
        [Display(Name = "HospitalId")]
        public Guid HospitalId { get; set; }

        [Required]
        [Display(Name = "ExamId")]
        public Guid ExamId { get; set; }
    }
}
