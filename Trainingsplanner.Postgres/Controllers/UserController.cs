using Duende.IdentityServer.Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private UserManager<ApplicationUser> UserManager { get; set; }

        public UserController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        [HttpGet("isAuthenticated")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> IsAuthenticated()
        {
            if (ModelState.IsValid)
            {
                return Ok(User.IsAuthenticated());
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("byId/{userId}")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanCreateContent)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(userId);
                user.PasswordHash = "";
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("byName/{name}")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanCreateContent)]
        public async Task<IActionResult> GetUserByName(string name)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(name);
                user.PasswordHash = "";
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("api/vi/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanAdministrate)]
        public async Task<IActionResult> AllowCreatContent(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var newClaim = new Claim(AppClaims.CanCreateContent, AppClaims.CanCreateContent);
            if ((await UserManager.GetClaimsAsync(user)).Contains(newClaim))
            {
                return Ok();
            }
            var result = await UserManager.AddClaimAsync(user, newClaim);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("api/vi/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanCreateContent)]
        public async Task<IActionResult> AllowReadGroup(int trainignsGroupId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var newClaim = new Claim(AppClaims.ReadTrainingsGroup, trainignsGroupId.ToString());
            if ((await UserManager.GetClaimsAsync(user)).Contains(newClaim))
            {
                return Ok();
            }
            var result = await UserManager.AddClaimAsync(user, newClaim);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanAdministrate)]
        public async Task<IActionResult> AllowEditGroup(int trainignsGroupId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var newClaim = new Claim(AppClaims.EditTrainingsGroup, trainignsGroupId.ToString());
            if ((await UserManager.GetClaimsAsync(user)).Contains(newClaim))
            {
                return Ok();
            }
            var result = await UserManager.AddClaimAsync(user, newClaim);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanCreateContent)]
        public async Task<IActionResult> AllowEditAppointment(int trainingsAppointmentId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var newClaim = new Claim(AppClaims.EditTrainingsAppointment, trainingsAppointmentId.ToString());
            if ((await UserManager.GetClaimsAsync(user)).Contains(newClaim))
            {
                return Ok();
            }
            var result = await UserManager.AddClaimAsync(user, newClaim);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanEditTrainingsModule)]
        public async Task<IActionResult> AllowEditModule(int trainingsModuleId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var newClaim = new Claim(AppClaims.EditTrainingsModule, trainingsModuleId.ToString());
            if ((await UserManager.GetClaimsAsync(user)).Contains(newClaim))
            {
                return Ok();
            }
            var result = await UserManager.AddClaimAsync(user, newClaim);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanAdministrate)]
        public async Task<IActionResult> DisallowEditModule(int moduleId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await UserManager.RemoveClaimAsync(user, new Claim(AppClaims.EditTrainingsModule, moduleId.ToString()));

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanAdministrate)]
        public async Task<IActionResult> DisallowEditAppointment(int appointmentId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await UserManager.RemoveClaimAsync(user, new Claim(AppClaims.EditTrainingsAppointment, appointmentId.ToString()));

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanAdministrate)]
        public async Task<IActionResult> DisallowEditGroup(int groupId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await UserManager.RemoveClaimAsync(user, new Claim(AppClaims.EditTrainingsGroup, groupId.ToString()));

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanCreateContent)]
        public async Task<IActionResult> DisallowReadGroup(int groupId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await UserManager.RemoveClaimAsync(user, new Claim(AppClaims.ReadTrainingsGroup, groupId.ToString()));

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppPolicies.CanAdministrate)]
        public async Task<IActionResult> DisallowCanCreateContent(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await UserManager.RemoveClaimAsync(user, new Claim(AppClaims.CanCreateContent, AppClaims.CanCreateContent));

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }

}
