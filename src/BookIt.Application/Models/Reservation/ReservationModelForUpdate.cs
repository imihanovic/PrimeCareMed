using BookIt.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.Models.Reservation
{
    public class ReservationModelForUpdate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Reservation status*")]
        public ReservationStatus Status { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Reservation details*")]
        public string ReservationDetails { get; set; }
    }
}
