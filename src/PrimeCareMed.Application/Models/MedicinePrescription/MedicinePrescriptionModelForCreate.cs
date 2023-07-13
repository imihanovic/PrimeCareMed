using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrimeCareMed.Application.Models.MedicinePrescription
{
    public class MedicinePrescriptionModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Medicine")]
        public string MedicineId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Appointment")]
        public string AppointmentId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Date Prescribed")]
        public DateTime DatePrescribed { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

    }
}
