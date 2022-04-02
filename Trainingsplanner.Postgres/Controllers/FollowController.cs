using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.BuisnessLogic.Mapping;
using Trainingsplanner.Postgres.Data.Models;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.ViewModels;

namespace Trainingsplanner.Postgres.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly ITrainingsModuleFollowRepository TrainingsModuleFollowRepository;
        private UserManager<ApplicationUser> UserManager { get; set; }
        public FollowController(ITrainingsModuleFollowRepository trainingsModuleFollowRepository, UserManager<ApplicationUser> userManager)
        {
            TrainingsModuleFollowRepository = trainingsModuleFollowRepository;
            UserManager = userManager;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TrainingsModuleFollowDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FollowModule(int moduleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userName = User.FindFirstValue(ClaimTypes.Email); // will give the user's userName
            var currentUser = await UserManager.FindByEmailAsync(userName);

            var ret = await TrainingsModuleFollowRepository.Follow(new TrainingsModuleFollow() { TrainingsModuleId = moduleId, UserId = currentUser.Id });

            return Ok(ret.ToViewModel());
        }

        [HttpDelete]
        [ProducesResponseType(typeof(TrainingsModuleFollowDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UnFollowModule(int moduleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userName = User.FindFirstValue(ClaimTypes.Email); // will give the user's userName
            var currentUser = await UserManager.FindByEmailAsync(userName);

            var ret = await TrainingsModuleFollowRepository.UnFollow(new TrainingsModuleFollow() { TrainingsModuleId = moduleId, UserId = currentUser.Id});

            return Ok(ret.ToViewModel());
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<TrainingsModuleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetFollowers(int trainingsModuleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsModuleId <= 0)
                return BadRequest();

            var tags = await TrainingsModuleFollowRepository.ReadTrainingsModuleFollowers(trainingsModuleId);

            if (tags == null)
            {
                return NotFound();
            }

            return Ok(tags);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<TrainingsModuleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetFollows(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (userId == null || userId.Length <= 0)
                return BadRequest();

            var tags = await TrainingsModuleFollowRepository.ReadFollowedTrainingsModules(userId);

            if (tags == null)
            {
                return NotFound();
            }

            return Ok(tags.Select(t => t.ToViewModel()));
        }
    }
}
