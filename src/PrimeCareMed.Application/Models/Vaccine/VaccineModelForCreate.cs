using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrimeCareMed.Application.Models.Vaccine
{
    public class VaccineModelForCreate
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Side effects")]
        public string SideEffects { get; set; }
    }
}
