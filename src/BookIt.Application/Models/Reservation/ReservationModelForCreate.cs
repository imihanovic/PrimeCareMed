using BookIt.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.Models.Reservation
{ 
    public class ReservationModelForCreate
    {
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "customer")]
        public string customerid { get; set; }

        [Required]
        [Range(1, 20)]
        [Display(Name = "Number of persons*")]
        public int NumberOfPerson { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date*")]
        public DateTime Date { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Time*")]
        public string Time { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Reservation status*")]
        public ReservationStatus Status { get; set; }

        [Required]
        public List<BookIt.Core.Entities.Table> Tables { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Reservation details")]
        public string ReservationDetails { get; set; }


    }
}
