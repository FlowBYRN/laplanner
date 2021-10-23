using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using StsServerIdentity.Models;

namespace StsServerIdentity.Data
{
    public class ProfileService : IProfileService
        {
            private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
            private readonly UserManager<ApplicationUser> _userManager;

            public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
            {
                _userManager = userManager;
                _claimsFactory = claimsFactory;
            }

            public async Task GetProfileDataAsync(ProfileDataRequestContext context)
            {
                var sub = context.Subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);
                var principal = await _claimsFactory.CreateAsync(user);

                var claims = principal.Claims.ToList();

                // // Wir können dynamisch Claims ergänzen
                // if (user.CanCreateUsers)
                // {
                //     claims.Add(new Claim(AppClaims.CanCreateUsersClaimType, "true"));
                // }

                context.IssuedClaims = claims;
            }

            public async Task IsActiveAsync(IsActiveContext context)
            {
                var sub = context.Subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);
                context.IsActive = user != null;
            }
        }
    }
