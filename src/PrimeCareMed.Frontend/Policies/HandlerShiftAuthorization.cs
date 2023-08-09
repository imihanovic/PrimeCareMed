using Microsoft.AspNetCore.Authorization;
using PrimeCareMed.DataAccess.Repositories;
using System.Security.Claims;

namespace PrimeCareMed.Frontend.Policies
{
    public class HandlerShiftAuthorization : AuthorizationHandler<AllowShiftAuthorizationRequirement>
    {
        private readonly IShiftRepository _shiftRepository;
        public HandlerShiftAuthorization(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowShiftAuthorizationRequirement requirement)
        {
            var user = context.User;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var isDoctor = user.IsInRole("Doctor");
            var isNurse = user.IsInRole("Nurse");
            if (isDoctor)
            {
                if (_shiftRepository.CheckIfOpenShiftExistsForDoctor(userId) is not null)
                {
                    context.Succeed(requirement);
                }
            }
            else if (isNurse)
            {
                if (_shiftRepository.CheckIfOpenShiftExistsForNurse(userId) is not null)
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
