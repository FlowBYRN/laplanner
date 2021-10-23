using System.Security.Claims;
using System.Threading.Tasks;
using EmailService;
using IdentityServer4;
using IdentityServerHost.Controllers.Extentions;
using IdentityServerHost.ViewModels.Account;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StsServerIdentity.Models;

namespace IdentityServerHost.Controllers
{
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [ApiController]
  //  [Route("api/v1/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private UserManager<ApplicationUser> UserManager { get; set; }
        private IEmailSender EmailSender { get; }
        public RegistrationController(UserManager<ApplicationUser> userManager,IEmailSender emailSender)
        {
            UserManager = userManager;
            EmailSender = emailSender;
        }
        
        [HttpPost("api/vi/[action]")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Policy = AppPolicies.CanAdministrate)]
        public async Task<IActionResult> RegisterTrainer(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Firstname + model.Lastname,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    PasswordReseted = false,
                    EmailConfirmed = false
                };
                string password = PasswordGenerator.GetRandomPassword(14);
                var result = await UserManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return NotFound(result.Errors);
                }
                await UserManager.AddClaimAsync(user, new Claim(AppClaims.CanCreateContent,AppClaims.CanCreateContent));
                if (user == null)
                {
                    return NotFound();
                }
                
                var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, Request.Scheme);


                string emailText =
                    $"Servus {user.FirstName},\r\n\r\ndu wurdest für den Trainingsplanner registriert, hier kannst du deine Trainingspläne einfach und digital verwalten und auch direkt das Training für die nächsten Wochen planen.\r\n\r\n" +
                    $"Hier ist dein aktuelles generiertes Password:    {password}   \r\nBitte Bestätige deine Registration über diesen Link:\r\n {confirmationLink}\r\n\r\nDort musst du noch dein Passwort ändern und du kannst loslegen";
                
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
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Policy = AppPolicies.CanCreateContent)]
        public async Task<IActionResult> RegisterAthlete(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Firstname +  model.Lastname,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    PasswordReseted = false,
                    EmailConfirmed = false
                };
                
                string password = PasswordGenerator.GetRandomPassword(14);
                var result = await UserManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return NotFound(result.Errors);
                }
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
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Policy = AppPolicies.CanAdministrate)]
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
        

    }
}