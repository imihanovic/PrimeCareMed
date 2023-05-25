using Microsoft.AspNetCore.Authorization;

namespace BookIt.Frontend.Policies
{
    public class AllowAnonymousAuthorizationRequirement :
        AuthorizationHandler<AllowAnonymousAuthorizationRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowAnonymousAuthorizationRequirement requirement)
        {
            var user = context.User;
            var userIsAnonymous = user?.Identity == null || !user.Identities.Any(i => i.IsAuthenticated);
            var isAdmin = user.IsInRole("Administrator");
            if (userIsAnonymous || isAdmin)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
