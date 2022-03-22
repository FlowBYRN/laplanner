using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Trainingsplanner.Postgres.BuisnessLogic.Mapping;
using Trainingsplanner.Postgres.Data.Models;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.ViewModels;

namespace Trainingsplanner.Postgres.Controllers
{
    [Authorize]
    [Route("api/v1/modules")]
    [ApiController]
    public class TrainingsModuleController : ControllerBase
    {
        private ITrainingsModuleRepository TrainingsModuleRepository { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        private IAuthorizationService AuthorizationService { get; set; }
        public TrainingsModuleController(ITrainingsModuleRepository trainingsModuleRepository, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            TrainingsModuleRepository = trainingsModuleRepository;
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }

        /// <summary>
        /// Creates a new TrainingsModule from Body
        /// </summary>
        /// <param name="trainingsModule">Id of the needed TrainingsModule</param>
        /// <returns>The TrainingsModule</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TrainingsModuleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> CreateTrainingsModule(TrainingsModuleDto trainingsModule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsModule == null)
            {
                return BadRequest();
            }

            var entity = await TrainingsModuleRepository.CreateTrainingsModule(trainingsModule.ToEntity());

            return CreatedAtAction(nameof(GetTrainingsModuleById), new { id = entity.Id }, entity.ToViewModel());
        }

        /// <summary>
        /// Adds Exercise to a specific Module
        /// </summary>
        /// <param name="moduleId">Id of the needed TrainingsModule</param>
        /// <param name="exerciseId">Id of the needed Exercise</param>
        /// <returns>TrainingsModuleTrainingsExerciseDto</returns>
        [HttpPost("exercisemodule")]
        [ProducesResponseType(typeof(TrainingsModuleTrainingsExerciseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> AddExerciseToModule(int moduleId, int exerciseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (moduleId <= 0 || exerciseId <= 0)
            {
                return BadRequest();
            }


            var tmp = await TrainingsModuleRepository.AddExerciseToModule(new Data.Models.TrainingsModuleTrainingsExercise() { TrainingsModuleId = moduleId, TrainingsExerciesId = exerciseId });

            return Created("", tmp.ToViewModel());
        }

        /// <summary>
        /// Adds Tag to a specific Module
        /// </summary>
        /// <param name="moduleId">Id of the needed TrainingsModule</param>
        /// <param name="tagId">the needed Exercise</param>
        /// <returns>TrainingsModuleTrainingsExerciseDto</returns>
        [HttpPost("/tagmodule")]
        [ProducesResponseType(typeof(TrainingsModuleTrainingsExerciseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> AddTagToModule(int moduleId, int tagId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (moduleId <= 0 || tagId <= 0)
            {
                return BadRequest();
            }


            var tmp = await TrainingsModuleRepository.AddTagToModule(new Data.Models.TrainingsModuleTrainingsModuleTag() { TrainingsModuleId = moduleId, TrainingsModuleTagId = tagId });

            return Created("", tmp);
        }

        /// <summary>
        /// Deletes the TrainingsModuleTag with the given Id from the given Module
        /// </summary>
        /// <param name="moduleId">The TrainingsModuleId (from HTTP-Body)</param>
        /// <param name="tagId">The TagId(from HTTP-Body)</param>
        /// <returns>The deleted TrainingsModule</returns>
        [HttpDelete("{moduleId}/tags/{tagId}")]
        [ProducesResponseType(typeof(TrainingsModuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTagByModuleId(int moduleId, int tagId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (moduleId < 0)
            {
                return BadRequest();
            }

            if (tagId < 0)
            {
                return BadRequest();
            }

            var entity = await TrainingsModuleRepository.DeleteTagFromModule(moduleId, tagId);

            return Ok(entity);
        }

        /// <summary>
        /// Updates the TrainingsModule with the given Id
        /// </summary>
        /// <param name="trainingsModule">The TrainingsModule with updated values (from HTTP-Body)</param>
        /// <returns>The updated TrainingsModule</returns>
        [HttpPut]
        [ProducesResponseType(typeof(TrainingsModuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateTrainingsModule(TrainingsModuleDto trainingsModule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsModule == null)
            {
                return BadRequest();
            }

            var entity = trainingsModule.ToEntity();
            var authorizationResult = await AuthorizationService.AuthorizeAsync(User, entity, AppPolicies.CanEditTrainingsModule);
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var result = await TrainingsModuleRepository.UpdateTrainingsModule(entity);
            return Ok(result.ToViewModel());
        }

        /// <summary>
        /// Deletes the TrainingsModule with the given Id
        /// </summary>
        /// <param name="trainingsModule">The TrainingsModule with updated values (from HTTP-Body)</param>
        /// <returns>The deleted TrainingsModule</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteTrainingsModule(TrainingsModuleDto trainingsModule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsModule == null)
            {
                return BadRequest();
            }

            var entity = trainingsModule.ToEntity();
            var authorizationResult = await AuthorizationService.AuthorizeAsync(User, entity, AppPolicies.CanEditTrainingsModule);
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var result = await TrainingsModuleRepository.DeleteTrainingsModule(entity);
            if (result == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{id}/exercises")]
        [ProducesResponseType(typeof(List<TrainingsExerciseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllExercisesByModuleId(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id <= 0)
            {
                return BadRequest();
            }

            var exercises = await TrainingsModuleRepository.ReadAllExercixesByModuleId(id);

            return Ok(exercises.Select(e => e.ToViewModel()));
        }


        /// <summary>
        /// Deletes the Exercise with the given Id from the Module with the given Id
        /// </summary>
        /// <param name="moduleId">The TrainingsModule with updated values (from HTTP-Body)</param>
        /// <param name="exerciseId">The TrainingsModule with updated values (from HTTP-Body)</param>
        /// <returns>The deleted TrainingsModule</returns>
        [HttpDelete("{moduleId}/exercises/{exerciseId}")]
        [ProducesResponseType(typeof(TrainingsModuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteExerciseByModuleId(int moduleId, int exerciseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (moduleId < 0)
            {
                return BadRequest();
            }

            if (exerciseId < 0)
            {
                return BadRequest();
            }

            var entity = await TrainingsModuleRepository.DeleteTrainingsModuleTrainingsExercise(moduleId, exerciseId);

            return Ok(entity.ToViewModel());
        }

        /// <summary>
        /// Returns all TrainingsModules
        /// </summary>
        /// <param name="trainingscModuleId">Id of the needed TrainingsModule</param>
        /// <returns>The TrainingsModule</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TrainingsModuleDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTrainingsModuleById(int trainingscModuleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingscModuleId <= 0)
            {
                return BadRequest();
            }

            var entity = await TrainingsModuleRepository.ReadTrainingsModuleById(trainingscModuleId);

            if (entity == null)
            {
                return NotFound();
            }

            var ret = entity.ToViewModel();
            return Ok(ret);
        }

        /// <summary>
        /// Returns the TrainingsModule with the given Id
        /// </summary>
        /// <param name="trainingscModuleId">Id of the needed TrainingsModule</param>
        /// <returns>The TrainingsModule</returns>
        [HttpGet("byUser/{userId}")]
        [ProducesResponseType(typeof(List<TrainingsModuleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTrainingsModulesByUserId(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var entity = await TrainingsModuleRepository.ReadTrainingsModulesByUserId(userId);

            if (entity == null)
            {
                return NotFound();
            }

            var ret = entity.Select(e => e.ToViewModel());
            return Ok(ret);
        }

        /// <summary>
        /// Returns all TrainingsModules that are part of the given appointment id
        /// </summary>
        /// <param name="trainingsAppointmentId">Id of the needed TrainingsAppointment</param>
        /// <returns>The TrainingsModule</returns>
        [HttpGet("appointments/{id}")]
        [ProducesResponseType(typeof(List<TrainingsModuleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTrainingsModuleByAppointmentId(int trainingsAppointmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsAppointmentId <= 0)
            {
                return BadRequest();
            }

            var entity = await TrainingsModuleRepository.ReadTrainingsModuleById(trainingsAppointmentId);

            if (entity == null)
            {
                return NotFound();
            }

            var ret = entity.ToViewModel();
            return Ok(ret);
        }

        /// <summary>
        /// Returns all TrainingsModules
        /// </summary>
        /// <returns>All TrainingsModules</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TrainingsModuleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllTrainingsModules()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var trainingsModels = await TrainingsModuleRepository.ReadAllTrainingsModule();
            return Ok(trainingsModels.Select(tm => tm.ToViewModel()));
        }

        /// <summary>
        /// Returns all TrainingsModules
        /// </summary>
        /// <returns>All TrainingsModules</returns>
        [HttpGet("public")]
        [ProducesResponseType(typeof(List<TrainingsModuleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllPublicTrainingsModules()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var trainingsModels = await TrainingsModuleRepository.ReadAllPublicTrainingsModule();
            foreach (var model in trainingsModels)
            {
                model.User = await this.UserManager.FindByIdAsync(model.UserId);
                if(model.User != null)
                    model.User.PasswordHash = null;
            }
            return Ok(trainingsModels.Select(tm => tm.ToViewModel()));
        }
    }
}