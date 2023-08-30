namespace PrimeCareMed.Application.Models.Shift
{
    public class ShiftModel : BaseResponseModel
    {
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public string NurseFirstName { get; set; }
        public string NurseLastName { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficeCity { get; set; }
        public object OfficeName { get; set; }
        public DateTime ShiftStartTime { get; set; }
#nullable enable
        public DateTime? ShiftEndTime { get; set; }
#nullable disable
    }
}
