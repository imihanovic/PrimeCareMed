using PrimeCareMed.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Models.HospitalCheckup
{
    public class HospitalCheckupModel
    {
        public Guid HospitalId { get; set; }
        public Guid CheckupId { get; set; }
        public string CheckupName { get; set; }
        public string HospitalName { get; set; }
        public string HospitalAddressCity { get; set; }

    }
}
