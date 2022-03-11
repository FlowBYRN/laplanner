using System.Threading.Tasks;
using Infrastructure;
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
            if (context.User.HasClaim(requirement.Claim, resource.Id.ToString()) || context.User.HasClaim(AppClaims.CanAdminsitrate, AppClaims.CanAdminsitrate))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}