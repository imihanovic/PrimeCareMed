using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrimeCareMed.Application.Models.Medicine
{
    public class MedicineModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
