using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.AuthorizationHandler
{
    public class TrainingsGroupRequirement : IAuthorizationRequirement
    {
        public TrainingsGroupRequirement(string claim)
        {
            Claim = claim;
        }
        public string Claim { get; private set; }
    }
    public class TrainingsGroupHandler : AuthorizationHandler<TrainingsGroupRequirement, TrainingsGroup>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TrainingsGroupRequirement requirement,
            TrainingsGroup resource)
        {
            if (context.User.HasClaim(requirement.Claim, resource.Id.ToString()) || context.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value == AppRoles.Admin)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}