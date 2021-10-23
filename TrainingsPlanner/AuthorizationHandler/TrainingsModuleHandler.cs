using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.AuthorizationHandler
{
    public class TrainingsModuleRequirement : IAuthorizationRequirement
    {
        
    }
    public class TrainingsModuleHandler: AuthorizationHandler<TrainingsModuleRequirement,TrainingsModule>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TrainingsModuleRequirement requirement,
            TrainingsModule resource)
        {
            if (context.User.HasClaim(AppClaims.EditTrainingsModule,resource.Id.ToString()) || context.User.HasClaim(AppClaims.CanAdminsitrate,AppClaims.CanAdminsitrate))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}