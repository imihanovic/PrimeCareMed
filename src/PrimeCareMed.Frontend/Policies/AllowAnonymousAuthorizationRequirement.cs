using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using PrimeCareMed.DataAccess.Repositories;
using System.Security.Claims;

namespace PrimeCareMed.Frontend.Policies
{
    public class AllowShiftAuthorizationRequirement : IAuthorizationRequirement
    {
        public AllowShiftAuthorizationRequirement()
        {
            
        }
    }
}
