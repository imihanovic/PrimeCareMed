using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Core.Enums
{
    public enum UserRole
    {
        [Display(Name = "SysAdministrator")]
        SysAdministrator,
        [Display(Name = "Administrator")]
        Administrator,
        [Display(Name = "Doctor")]
        Doctor,
        [Display(Name = "Nurse")]
        Nurse
    }
}
