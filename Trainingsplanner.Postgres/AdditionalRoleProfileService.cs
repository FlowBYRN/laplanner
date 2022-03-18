using Duende.IdentityServer;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres
{
    public class AdditionalRoleProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdditionalRoleProfileService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _claimsFactory = claimsFactory;
        }

        async Task IProfileService.GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(sub);

            List<Claim> claims = new();
            claims.AddRange(await _userManager.GetClaimsAsync(user));

            IList<string> roles = await _userManager.GetRolesAsync(user);
            
            foreach (string role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            claims.Add(new Claim(JwtClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"));

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
            }
            context.IssuedClaims.AddRange(claims);
        }

        async Task IProfileService.IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
