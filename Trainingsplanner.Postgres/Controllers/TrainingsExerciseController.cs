using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.ViewModels;
using Trainingsplanner.Postgres.BuisnessLogic.Mapping;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Controllers
{
    [Route("/api/v1/trainingsexercises")]
    [ApiController]
    public class TrainingsExerciseController : ControllerBase
    {
        private ITrainingsExerciseRepository TrainingsExerciseRepository { get; set; }
        private IAuthorizationService AuthorizationService { get; set; }

        public TrainingsExerciseController(ITrainingsExerciseRepository trainingsExerciseRepository, IAuthorizationService authorizationService)
        {
            TrainingsExerciseRepository = trainingsExerciseRepository;
            AuthorizationService = authorizationService;
        }

        /// <summary>
        /// Creates new TrainingsExercise from Body
        /// </summary>
        /// <param name="trainingsExercise">New TrainingsExercise from HTTP-Body</param>
        /// <returns>The newly created TrainingsExercise</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TrainingsExerciseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> CreateExercise(TrainingsExerciseDto trainingsExercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsExercise == null)
            {
                return BadRequest();
            }

            var entity = await TrainingsExerciseRepository.CreateTrainingsExercise(trainingsExercise.ToEntity());

            return CreatedAtAction(nameof(ReadExerciseById), new { id = entity.Id }, entity.ToViewModel());
        }

        /// <summary>
        /// Returns all Exercises
        /// </summary>
        /// <returns>All TrainingsExercises</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TrainingsExerciseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ReadAllExercises()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            List<TrainingsExercise> trainingsExercises = await TrainingsExerciseRepository.ReadAllTrainingsExercises();

            return Ok(trainingsExercises.Select(item => item.ToViewModel()));
        }

        /// <summary>
        /// Returns the TrainingsExercise with the given Id
        /// </summary>
        /// <param name="trainingsExerciseId">Id of the required TrainingsExercise</param>
        /// <returns>The TrainingsExercise</returns>
        [HttpGet("{trainingsExerciseId}")]
        [ProducesResponseType(typeof(TrainingsExerciseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReadExerciseById(int trainingsExerciseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsExerciseId <= 0)
            {
                return BadRequest();
            }

            var entity = await TrainingsExerciseRepository.ReadTrainingsExerciseById(trainingsExerciseId);
            var ret = entity.ToViewModel();
            return Ok(ret);
        }

        /// <summary>
        /// Returns the TrainingsExercise with the given Id
        /// </summary>
        /// <param name="trainingsModuleId">Id of the required TrainingsModule</param>
        /// <returns>The TrainingsExercise</returns>
        [HttpGet("modules/{trainingsModuleId}")]
        [ProducesResponseType(typeof(List<TrainingsExerciseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReadExercisesByModuleId(int trainingsModuleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsModuleId <= 0)
            {
                return BadRequest();
            }

            var entity = await TrainingsExerciseRepository.ReadTrainingsExercisesByTrainingsModuleId(trainingsModuleId);

            return Ok(entity.Select(item => item.ToViewModel()));
        }

        /// <summary>
        /// Updates the TrainingsExercise with the given Id
        /// </summary>
        /// <param name="trainingsExercise">The TrainingsExercise with updated values (from HTTP-Body)</param>
        /// <returns>The updated TrainingsExercise</returns>
        [HttpPut]
        [ProducesResponseType(typeof(TrainingsExerciseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> UpdateExercise(TrainingsExerciseDto trainingsExercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsExercise == null)
            {
                return BadRequest();
            }

            var entity = await TrainingsExerciseRepository.UpdateTrainingsExercise(trainingsExercise.ToEntity());
            return Ok(entity.ToViewModel());
        }

        /// <summary>
        /// Deletes the given TrainingsExercise
        /// </summary>
        /// <param name="trainingsExercise">The TrainingsExercise to delete</param>
        /// <returns>The deleted TrainingsExercise</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(TrainingsExerciseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> DeleteExercise(TrainingsExerciseDto trainingsExercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsExercise == null)
            {
                return BadRequest();
            }

            var entity = await TrainingsExerciseRepository.DeleteTrainingsExercise(trainingsExercise.ToEntity());
            var ret = entity.ToViewModel();
            return Ok(ret);
        }
    }
}
