using PrimeCareMed.Core.Common;

namespace PrimeCareMed.Core.Entities
{
    public class Exam : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Preparation { get; set; }
#nullable enable
        public ICollection<HospitalExam>? HospitalExams { get; set; } = new List<HospitalExam>();
#nullable disable
    }
}
