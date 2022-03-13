using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Migrations;

namespace Trainingsplanner.Postgres.AuthorizationHandler
{
    internal class HasRoleHandler : AuthorizationHandler<HasRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasRoleRequirement requirement)
        {
            if (context.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value == AppRoles.Admin)
                context.Succeed(requirement);

            if (context.User.IsInRole(requirement.RoleName))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    internal class HasRoleRequirement : IAuthorizationRequirement
    {
        public readonly string RoleName;

        public HasRoleRequirement(string roleName)
        {
            RoleName = roleName;
        }
    }
}

