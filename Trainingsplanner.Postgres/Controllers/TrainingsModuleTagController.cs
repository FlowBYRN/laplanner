using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.ViewModels;
using Trainingsplanner.Postgres.BuisnessLogic.Mapping;

namespace Trainingsplanner.Postgres.Controllers
{
    [Authorize]
    [Route("api/v1/tags")]
    [ApiController]
    public class TrainingsModuleTagController : ControllerBase
    {

        private ITrainingsModuleTagRepository TrainingsModuleTagRepository { get; set; }

        public TrainingsModuleTagController(ITrainingsModuleTagRepository trainingsModuleTagRepository)
        {
            TrainingsModuleTagRepository = trainingsModuleTagRepository;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<TrainingsModuleTagDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await TrainingsModuleTagRepository.ReadAllTags();

            if (tags == null)
            {
                return NotFound();
            }

            return Ok(tags.Select(t => t.ToViewModel()));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TrainingsModuleTagDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTagById(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var tag = await TrainingsModuleTagRepository.ReadTagById(id);

            if (tag == null)
            {
                return NotFound();
            }

            var tagDto = tag.ToViewModel();

            return Ok(tagDto);
        }


        [HttpPost]
        [ProducesResponseType(typeof(TrainingsModuleTagDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> CreateTag(TrainingsModuleTagDto trainingsModuleTagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsModuleTagDto == null)
            {
                return BadRequest();
            }

            var tag = await TrainingsModuleTagRepository.InsertTag(trainingsModuleTagDto.ToEntity());

            if (tag == null)
            {
                return NotFound();
            }

            var tagDto = tag.ToViewModel();

            return Ok(tagDto);
        }


        [HttpPut]
        [ProducesResponseType(typeof(TrainingsModuleTagDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> UpdateTag(TrainingsModuleTagDto trainingsModuleTagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsModuleTagDto == null)
            {
                return BadRequest();
            }

            var tag = await TrainingsModuleTagRepository.UpdateTag(trainingsModuleTagDto.ToEntity());

            if (tag == null)
            {
                return NotFound();
            }

            var tagDto = tag.ToViewModel();

            return Ok(tagDto);
        }


        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [Authorize(Policy = AppRoles.Trainer)]
        public async Task<IActionResult> DeleteTag(TrainingsModuleTagDto trainingsModuleTagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (trainingsModuleTagDto == null)
            {
                return BadRequest();
            }

            var tag = await TrainingsModuleTagRepository.DeleteTag(trainingsModuleTagDto.ToEntity());

            if (tag == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
