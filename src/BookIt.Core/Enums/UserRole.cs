using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Core.Enums
{
    public enum UserRole
    {
        [Display(Name = "Administrator")]
        Administrator,
        [Display(Name = "Manager")]
        Manager,
        [Display(Name = "Customer")]
        Customer
    }
}
