using PrimeCareMed.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrimeCareMed.Core.Entities
{
    public class HospitalExam
    {
        public Guid HospitalId { get; set; }

        public Guid ExamId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }
#nullable enable
        public ICollection<ExamAppointment>? HospitalExams { get; set; } = new List<ExamAppointment>();
#nullable disable

    }
}
