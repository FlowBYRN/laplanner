using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http;
using Trainingsplanner.Postgres.ViewModels;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.BuisnessLogic.Mapping;
using Trainingsplanner.Postgres.Data.Models;
using Infrastructure;

namespace Trainingsplanner.Postgres.Controllers
{
    [Authorize]
    [Route("api/v1/appointments")]
    [ApiController]
    public class TrainingsAppointmentController : ControllerBase
    {
        private ITrainigsAppointmentRepository TrainigsAppointmentRepository { get; set; }
        private IAuthorizationService AuthorizationService { get; set; }

        public TrainingsAppointmentController(ITrainigsAppointmentRepository trainigsAppointmentRepository, IAuthorizationService authorizationService)
        {
            TrainigsAppointmentRepository = trainigsAppointmentRepository;
            AuthorizationService = authorizationService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<TrainingsAppointmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await TrainigsAppointmentRepository.ReadAllAppointments();

            return Ok(appointments);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TrainingsAppointmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id <= 0)
                return BadRequest();

            var appointment = await TrainigsAppointmentRepository.ReadAppointmentById(id);

            if (appointment == null)
                return NotFound();

            return Ok(appointment.ToViewModel());
        }

        [HttpGet("{id}/full")]
        [ProducesResponseType(typeof(TrainingsAppointmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetFullAppointmentById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id <= 0)
                return BadRequest();

            var appointment = await TrainigsAppointmentRepository.ReadFullAppointmentById(id);

            if (appointment == null)
                return NotFound();

            return Ok(appointment.ToViewModel());
        }
        /// <summary>
        /// Returns the TrainingsAppointments of spcified User
        /// </summary>
        /// <param name="trainingscModuleId">Id of the needed TrainingsModule</param>
        /// <returns>The TrainingsModule</returns>
        [HttpGet("byUser/{userId}")]
        [ProducesResponseType(typeof(List<TrainingsAppointmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTrainingsAppointmentsByUserId(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var entity = await TrainigsAppointmentRepository.ReadTrainingsAppointmentsByUserId(userId);

            if (entity == null)
            {
                return NotFound();
            }

            var ret = entity.Select(e => e.ToViewModel());
            return Ok(ret);
        }
        [HttpGet("{id}/modules")]
        [ProducesResponseType(typeof(List<TrainingsModuleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetModulesByAppointmentId(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id <= 0)
                return BadRequest();

            var entites = await TrainigsAppointmentRepository.ReadModulesByAppointmentId(id);

            return Ok(entites.Select(e => e.ToViewModel()));
        }

        [HttpPost]
        [ProducesResponseType(typeof(TrainingsAppointmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> CreateAppointment([FromBody] TrainingsAppointmentDto trainingsAppointment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsAppointment == null)
            {
                return BadRequest();
            }

            var appointment = await TrainigsAppointmentRepository.CreateAppointment(trainingsAppointment.ToEntity());

            if (appointment == null)
            {
                return NotFound();
            }

            var appointmentDto = appointment.ToViewModel();

            return Created("", appointmentDto);
        }

        [HttpPost("{trainingsAppointmentId}")]
        [ProducesResponseType(typeof(List<TrainingsAppointmentTrainingsModuleDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> AddModuleToAppointment(int trainingsAppointmentId, [FromBody] List<int> moduleIds)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (moduleIds == null || moduleIds.Count == 0 || trainingsAppointmentId <= 0)
            {
                return BadRequest();
            }

            // var result = await AuthorizationService.AuthorizeAsync(User, new TrainingsAppointment(){Id = trainingsAppointmentId}, AppPolicies.CanEditTrainingsAppointment);
            // if (!result.Succeeded)
            // {
            //     return Forbid();
            // }

            List<TrainingsAppointmentTrainingsModuleDto> listAM = new List<TrainingsAppointmentTrainingsModuleDto>();
            var existing = await this.TrainigsAppointmentRepository.ReadModulesByAppointmentId(trainingsAppointmentId);

            var toCreate = moduleIds.Except(existing?.Select(m => m.Id));
            var toDelete = existing?.Select(m => m.Id).Except(moduleIds);

            foreach (var module in toCreate)
            {
                TrainingsAppointmentTrainingsModuleDto appointmentM = new TrainingsAppointmentTrainingsModuleDto();
                appointmentM.TrainingsAppointmentId = trainingsAppointmentId;
                appointmentM.TrainingsModuleId = module;

                var tmp = await TrainigsAppointmentRepository.AddModuleToAppointment(appointmentM.ToEntity());

                if (tmp != null)
                {
                    listAM.Add(tmp.ToViewModel());
                }
            }

            foreach (var item in toDelete)
            {
                TrainingsAppointmentTrainingsModuleDto appointmentM = new TrainingsAppointmentTrainingsModuleDto();
                appointmentM.TrainingsAppointmentId = trainingsAppointmentId;
                appointmentM.TrainingsModuleId = item;

                var tmp = await TrainigsAppointmentRepository.DeleteModuleFromAppointment(trainingsAppointmentId,item);
            }

            return Created("", listAM);
        }

        [HttpPut]
        [ProducesResponseType(typeof(TrainingsAppointmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAppointment([FromBody] TrainingsAppointmentDto trainingsAppointment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await AuthorizationService.AuthorizeAsync(User, trainingsAppointment.ToEntity(), AppPolicies.CanEditTrainingsAppointment);
            if (!result.Succeeded)
            {
                return Forbid();
            }
            if (trainingsAppointment == null)
            {
                return BadRequest();
            }

            var appointment = await TrainigsAppointmentRepository.UpdateAppointment(trainingsAppointment.ToEntity());

            if (appointment == null)
            {
                return NotFound();
            }

            var appointmentDto = appointment.ToViewModel();

            return Ok(appointmentDto);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAppointment(TrainingsAppointmentDto trainingsAppointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await AuthorizationService.AuthorizeAsync(User, trainingsAppointment.ToEntity(), AppPolicies.CanEditTrainingsAppointment);
            if (!result.Succeeded)
            {
                return Forbid();
            }

            if (trainingsAppointment == null)
            {
                return BadRequest();
            }

            var appointment = await TrainigsAppointmentRepository.DeleteAppointment(trainingsAppointment.ToEntity());

            if (appointment == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{appointmentId}/modules/{moduleId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAppointmentWithModule(int appointmentId, int moduleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (appointmentId < 0)
            {
                return BadRequest();
            }

            if (moduleId < 0)
            {
                return BadRequest();
            }

            var result = await AuthorizationService.AuthorizeAsync(User, new TrainingsAppointment() { Id = appointmentId }, AppPolicies.CanEditTrainingsAppointment);
            if (!result.Succeeded)
            {
                return Forbid();
            }

            var appointmentModule = await TrainigsAppointmentRepository.DeleteModuleFromAppointment(appointmentId, moduleId);

            if (appointmentModule == null)
            {
                return Problem("Appointment could not be deleted!", "Appointment", 500, "Appointment deleting");
            }

            return Ok();
        }
    }
}
