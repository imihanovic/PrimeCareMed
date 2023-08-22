namespace PrimeCareMed.Application.Models.Checkup
{
    public class CheckupModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Preparation { get; set; }
    }
}
