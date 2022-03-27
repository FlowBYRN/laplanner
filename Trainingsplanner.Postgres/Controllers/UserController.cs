using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using EmailService;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.BuisnessLogic;
using Trainingsplanner.Postgres.Data.Models;
using Trainingsplanner.Postgres.DataAccess;
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

        private ITrainingsGroupRepository TrainingsGroupRepository { get; set; }

        private IEmailSender EmailSender { get; }

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, ITrainingsGroupRepository trainingsGroupRepository)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            EmailSender = emailSender;
            TrainingsGroupRepository = trainingsGroupRepository;
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
                user = await UserManager.FindByNameAsync(user.UserName);

                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code, },
                protocol: Request.Scheme);


                string emailText =
                    $"Servus {user.FirstName},<br><br>du wurdest für den Leichtathletik-Trainingsplanner registriert, hier kannst du deine Trainingspläne einfach und digital verwalten und einfach per Drag&Drop das Training für die nächsten Wochen planen.\r\n\r\n" +
                   $"Bitte Bestätige deine Registration:  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Hier</a> <br><br><h4>Bitte ändere dann unbedingt dein Passwort</h4><br>Viel Spaß beim planen :D";

                var message = new Message(new string[] { user.Email }, "Einladung zum Trainingsplanner: Registriere dich jetzt", emailText, null);
                await EmailSender.SendEmailAsync(message);


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

                var userId = await UserManager.GetUserIdAsync(user);
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code, },
                    protocol: Request.Scheme);


                string emailText =
                    $"Servus {user.FirstName},<br><br>du wurdest für den Trainingsplanner registriert, hier kannst du Trainingspläne deiner Sport-Gruppe finden und anschauen.<br>" +
                   $"Bitte Bestätige deine Registration:  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Hier</a> <br><br><h4>Bitte ändere dann unbedingt dein Passwort</h4><br>Viel Spaß beim trainieren :D";

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
                if (user == null)
                    return NotFound();

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
                var split = name.Split(' ');
                if (split.Length < 2)
                    return BadRequest();

                string firstName = split[0];
                string lastName = split[1];

                var user = UserManager.Users.Where(u => u.FirstName == firstName && u.LastName == lastName).FirstOrDefault();

                if (user == null)
                    return NotFound();
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

        [HttpGet("byEmail/{email}")]
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
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> AllowEditAppointment(int trainingsAppointmentId, int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userIds = await TrainingsGroupRepository.ReadTrainerIdsByGroup(groupId);
            foreach (TrainingsGroupApplicationUser userId in userIds)
            {
                var user = await UserManager.FindByIdAsync(userId.ApplicationUserId);
                if (user != null)
                {
                    var newClaim = new Claim(AppClaims.EditTrainingsAppointment, trainingsAppointmentId.ToString());
                    if (!(await UserManager.GetClaimsAsync(user)).Contains(newClaim))
                    {
                        await UserManager.AddClaimAsync(user, newClaim);
                    }
                }
            }
                return Ok();
        }

        [HttpPost("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppRoles.Trainer)]
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
        [Authorize(Policy = AppRoles.Trainer)]
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
