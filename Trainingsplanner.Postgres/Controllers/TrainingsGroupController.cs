using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.ViewModels;
using Trainingsplanner.Postgres.BuisnessLogic.Mapping;

namespace Trainingsplanner.Postgres.Controllers
{
    [Authorize]
    [Route("api/v1/groups")]
    [ApiController]
    public class TrainingsGroupController : ControllerBase
    {
        private ITrainingsGroupRepository TrainingsGroupRepository { get; set; }
        private IAuthorizationService AuthorizationService { get; set; }

        public TrainingsGroupController(ITrainingsGroupRepository trainingsGroupTrainingsGroupRepository, IAuthorizationService authorizationService)
        {
            TrainingsGroupRepository = trainingsGroupTrainingsGroupRepository;
            AuthorizationService = authorizationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TrainingsGroupDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllGroups()
        {

            var allGroups = await TrainingsGroupRepository.ReadAllGroups();

            var groupsDto = allGroups
                .Where(group => AuthorizationService.AuthorizeAsync(User, group, AppPolicies.CanReadTrainingsGroup).Result.Succeeded)
                .Select(group => group.ToViewModel());

            return Ok(groupsDto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TrainingsGroupDto), StatusCodes.Status200OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await TrainingsGroupRepository.ReadGroupById(id);

            var authorizationResult = await AuthorizationService.AuthorizeAsync(User, group, AppPolicies.CanReadTrainingsGroup);
            if (authorizationResult.Succeeded)
            {
                return Ok(group.ToViewModel());
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpGet("{id}/appointments")]
        [ProducesResponseType(typeof(List<TrainingsAppointmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAppointmentsByGroupId(int id)
        {
            var appointments = await TrainingsGroupRepository.ReadAppointmentsByGroupId(id);

            return Ok(appointments.Select(a => a.ToViewModel()));
        }

        [HttpPost]
        [ProducesResponseType(typeof(TrainingsGroupDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [Authorize(Policy = AppRoles.Admin)]
        public async Task<IActionResult> CreateGroup(TrainingsGroupDto trainingsGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsGroupDto == null)
            {
                return BadRequest();
            }

            var group = await TrainingsGroupRepository.CreateGroup(trainingsGroupDto.ToEntity());
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group.ToViewModel());
        }

        [HttpPut]
        [ProducesResponseType(typeof(TrainingsGroupDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> UpdateGroup(TrainingsGroupDto trainingsGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsGroupDto == null)
            {
                return BadRequest();
            }

            var group = await TrainingsGroupRepository.UpdateGroup(trainingsGroupDto.ToEntity());

            if (group == null)
            {
                return NotFound();
            }
            return Ok(group.ToViewModel());
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> DeleteGroup(TrainingsGroupDto trainingsGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsGroupDto == null)
            {
                return BadRequest();
            }

            var group = TrainingsGroupRepository.DeleteGroup(trainingsGroupDto.ToEntity());
            if (group == null)
            {
                NotFound();
            }
            return NoContent();
        }

    }
}
