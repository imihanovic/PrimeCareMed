using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrimeCareMed.Application.Models.MedicinePrescription
{
    public class MedicinePrescriptionModel : BaseResponseModel
    {
        public string MedicineName { get; set; }
        public string MedicineDescription { get; set; }
        public DateTime DatePrescribed { get; set; }
        public string Description { get; set; }
    }
}
