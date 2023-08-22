using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Enums;

namespace PrimeCareMed.Core.Entities
{
    public class ExamAppointment : BaseEntity
    {
        public HospitalExam HospitalExam { get; set; }
        public Appointment Appointment { get; set; }
        public DateTime ExamDate { get; set; }
        public ExamStatus ExamStatus { get; set; }
    }
}
