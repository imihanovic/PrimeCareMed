namespace PrimeCareMed.Application.Models.Exam
{
    public class ExamModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Preparation { get; set; }
    }
}
