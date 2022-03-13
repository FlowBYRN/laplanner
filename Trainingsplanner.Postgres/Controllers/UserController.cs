using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using EmailService;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.BuisnessLogic;
using Trainingsplanner.Postgres.Data.Models;
using Trainingsplanner.Postgres.ViewModels;

namespace Trainingsplanner.Postgres.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private UserManager<ApplicationUser> UserManager { get; set; }
        private RoleManager<IdentityRole> RoleManager { get; set; }
        private IEmailSender EmailSender { get; }

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            UserManager = userManager;
            RoleManager = roleManager;    
            EmailSender = emailSender;
        }

        [HttpPost("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Policy = AppRoles.Admin)]
        public async Task<IActionResult> RegisterTrainer(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    EmailConfirmed = false
                };
                string password = PasswordGenerator.GetRandomPassword(14);
                var result = await UserManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return NotFound(result.Errors);
                }
                await UserManager.AddToRoleAsync(user, AppRoles.Trainer);
                if (user == null)
                {
                    return NotFound();
                }

                var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, Request.Scheme);


                string emailText =
                    $"Servus {user.FirstName},\r\n\r\ndu wurdest für den Trainingsplanner registriert, hier kannst du deine Trainingspläne einfach und digital verwalten und auch direkt das Training für die nächsten Wochen planen.\r\n\r\n" +
                    $"Hier ist dein aktuelles generiertes Password:    {password}   \r\nBitte Bestätige deine Registration über diesen Link:\r\n {confirmationLink}\r\n\r\nBitte ändere noch dein Passwort";

                var message = new Message(new string[] { user.Email }, "Einladung zum Trainingsplanner: Registriere dich jetzt", emailText, null);
                await EmailSender.SendEmailAsync(message);


                user = await UserManager.FindByNameAsync(user.UserName);
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Policy = AppRoles.Trainer)]
       public async Task<IActionResult> RegisterAthlete(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    EmailConfirmed = false
                };
                string password = PasswordGenerator.GetRandomPassword(14);
                var result = await UserManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return NotFound(result.Errors);
                }
                await UserManager.AddToRoleAsync(user, AppRoles.Athlet);
                user = await UserManager.FindByNameAsync(user.UserName);
                if (user == null)
                {
                    return NotFound();
                }
                var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, Request.Scheme);


                string emailText =
                    $"Servus {user.FirstName},\n\ndu wurdest für den Trainingsplanner registriert, hier kannst du Trainingspläne deiner Sport-Gruppe finden und anschauen.\n\n" +
                    $"Hier ist dein aktuelles generiertes Password:    {password}   \r\nBitte Bestätige deine Registration über diesen Link:\n {confirmationLink}\n\nDort musst du noch dein Passwort ändern und du kannst loslegen";

                var message = new Message(new string[] { user.Email }, "Finde deinen Trainingsplan: Registriere dich jetzt", emailText, null);
                await EmailSender.SendEmailAsync(message);


                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Policy = AppRoles.Admin)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(userId);

                var result = await UserManager.RemoveClaimsAsync(user, await UserManager.GetClaimsAsync(user));
                if (!result.Succeeded)
                {
                    NotFound();
                }
                result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    NotFound();
                }
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }

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
        [Authorize(Policy = AppRoles.Trainer)]
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
        [Authorize(Policy = AppRoles.Trainer)]
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

        [HttpGet("api/vi/[action]")]
        [ProducesResponseType(typeof(List<ApplicationUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppRoles.Admin)]
        public async Task<IActionResult> GetTrainers()
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUsersInRoleAsync(AppRoles.Trainer);
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet("byEmail/{name}")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(email);
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
        [Authorize(Policy = AppRoles.Trainer)]
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
        [Authorize(Policy = AppRoles.Admin)]
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
        [Authorize(Policy = AppRoles.Trainer)]
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
        [Authorize(Policy = AppRoles.Admin)]
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
        [Authorize(Policy = AppRoles.Admin)]
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
        [Authorize(Policy = AppRoles.Admin)]
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
        [Authorize(Policy = AppRoles.Trainer)]
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
    }

}
