using System.Net;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trainingsplanner.Postgres.BuisnessLogic.Mapping;
using Trainingsplanner.Postgres.Data.Models;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.ViewModels;

namespace Trainingsplanner.Postgres.Controllers
{
    [Authorize]
    [Route("api/v1/groupusers")]
    [ApiController]
    public class TrainingsGroupUserController : ControllerBase
    {
        private ITrainingsGroupUserRepository TrainingsGroupUserRepository { get; set; }
        private IAuthorizationService AuthorizationService { get; set; }

        public TrainingsGroupUserController(ITrainingsGroupUserRepository trainingsGroupUserRepository, IAuthorizationService authorizationService)
        {
            TrainingsGroupUserRepository = trainingsGroupUserRepository;
            AuthorizationService = authorizationService;
        }

        [HttpPost("athlete")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> AddUserToGroup(TrainingsGroupApplicationUserDto trainingsGroupApplicationUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await AuthorizationService.AuthorizeAsync(User, new TrainingsGroup() { Id = trainingsGroupApplicationUserDto.TrainingsGroupId }, AppClaims.EditTrainingsGroup);
            if (!result.Succeeded)
            {
                return Forbid();
            }

            return Ok(await TrainingsGroupUserRepository.CreateNewUserForGroup(trainingsGroupApplicationUserDto.ToEntity()));
        }

        [HttpPost("trainer")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = AppRoles.Admin)]
        public async Task<IActionResult> AddTrainerToGroup(TrainingsGroupApplicationUserDto trainingsGroupApplicationUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await AuthorizationService.AuthorizeAsync(User, new TrainingsGroup() { Id = trainingsGroupApplicationUserDto.TrainingsGroupId }, AppClaims.EditTrainingsGroup);
            if (!result.Succeeded)
            {
                return Forbid();
            }

            return Ok(await TrainingsGroupUserRepository.CreateNewTrainerForGroup(trainingsGroupApplicationUserDto.ToEntity()));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> DeleteMemberFromGroup(int trainingsGroupId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await AuthorizationService.AuthorizeAsync(User, new TrainingsGroup() { Id = trainingsGroupId }, AppClaims.EditTrainingsGroup);
            if (!result.Succeeded)
            {
                return Forbid();
            }

            return Ok(await TrainingsGroupUserRepository.DeleteMemberForGroup(trainingsGroupId, userId));
        }
    }
}