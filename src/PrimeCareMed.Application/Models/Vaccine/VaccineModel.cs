using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Models.Vaccine
{
    public class VaccineModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string SideEffects { get; set; }
    }
}
