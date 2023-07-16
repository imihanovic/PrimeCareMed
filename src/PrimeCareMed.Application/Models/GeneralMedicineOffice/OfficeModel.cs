using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Models.GeneralMedicineOffice
{
    public class OfficeModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }
}
