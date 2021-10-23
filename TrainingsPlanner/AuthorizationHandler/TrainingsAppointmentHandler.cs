using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.AuthorizationHandler
{
    public class TrainingsAppointmentRequirement : IAuthorizationRequirement
    {
    }

    public class TrainingsAppointmentHandler : AuthorizationHandler<TrainingsAppointmentRequirement,TrainingsAppointment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TrainingsAppointmentRequirement requirement,
            TrainingsAppointment resource)
        {
            if (context.User.HasClaim(AppClaims.EditTrainingsAppointment,resource.Id.ToString()) || context.User.HasClaim(AppClaims.CanAdminsitrate,AppClaims.CanAdminsitrate))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;        }
    }
}