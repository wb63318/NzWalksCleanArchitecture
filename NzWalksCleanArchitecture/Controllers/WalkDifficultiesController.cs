using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NzWalksCleanArchitecture.Application.Interfaces;
using NzWalksCleanArchitecture.Application.Repositories;
using System.Data;

namespace NzWalksCleanArchitecture.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository _difficultyRepository;
        private readonly IMapper _mapper;
        public WalkDifficultiesController(IMapper mapper, IWalkDifficultyRepository difficultyRepository)
        {
            _mapper = mapper;
            _difficultyRepository = difficultyRepository;
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody]Domain.Requests.AddWalkDifficulty walkDifficulty)
        {
            var walkDifficultyDomain = new Domain.Models.WalkDifficulty
            {
                Code = walkDifficulty.Code,
            };

            walkDifficultyDomain = await _difficultyRepository.AddAsync(walkDifficultyDomain);

            var walkDifficultyDto = _mapper.Map<Domain.Dtos.WalkDifficulty>(walkDifficultyDomain);
            return CreatedAtAction(nameof(GetWalkDifficultyById), new{id = walkDifficultyDto.Id },    
            walkDifficultyDto);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
       
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await _difficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // Convert Domain to DTOs
            var walkDifficultyDTO = _mapper.Map<Domain.Dtos.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWallDifficulties()
        {
            var walkDifficultiesDomain = await _difficultyRepository.GetAllAsync();

            var walkDifficultiesDto = _mapper.Map<List<Domain.Dtos.WalkDifficulty>>(walkDifficultiesDomain);

            return Ok(walkDifficultiesDto); 
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute]Guid id, [FromBody]Domain.Requests.UpdateWalkDifficulty updateWalkDifficulty
            )
        {
            // Validate the incoming request
            

            // Convert DTO to Domiain Model
            var walkDifficultyDomain = new Domain.Models.WalkDifficulty
            {
                Code = updateWalkDifficulty.Code
            };

            // Call repository to update
            walkDifficultyDomain = await _difficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO
            var walkDifficultyDTO = _mapper.Map<Domain.Dtos.WalkDifficulty>(walkDifficultyDomain);

            // Return response
            return Ok(walkDifficultyDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkDifficultyDomain = await _difficultyRepository.DeleteAsync(id);
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // Convert to DTO
            var walkDifficultyDTO = _mapper.Map<Domain.Dtos.WalkDifficulty>(walkDifficultyDomain);
            return Ok(walkDifficultyDTO);
        }
    }
}
