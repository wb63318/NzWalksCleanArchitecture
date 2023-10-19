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
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        private readonly IRegionRepository _regionRepository;
        private readonly IWalkDifficultyRepository _difficultyRepository;

        public WalksController(IWalkRepository walkRepository,IMapper mapper,IRegionRepository regionRepository,IWalkDifficultyRepository difficultyRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
            _regionRepository = regionRepository;
            _difficultyRepository = difficultyRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody]Domain.Requests.AddWalkRequest addWalk)
        {
            //validate incoming request


            var walkDomain = new Domain.Models.Walk
            {
                Length = addWalk.Length,
                Name = addWalk.Name,
                RegionId = addWalk.RegionId,
                WalkDifficultyId = addWalk.WalkDifficultyId,
            };

            walkDomain = await _walkRepository.AddAsync(walkDomain);

            var walkDto = new Domain.Dtos.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };

            return CreatedAtAction(nameof(GetWalkAsync),new { id = walkDto.Id}, walkDto);

        }
        [HttpGet("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // Get Walk Domain object from database
            var walkDomin = await _walkRepository.GetAsync(id);

            // Convert Domain object to DTO
            var walkDTO = _mapper.Map<Domain.
                Dtos.Walk>(walkDomin);

            // Return response
            return Ok(walkDTO);
        }
        [HttpGet]
       // [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database - domain walks
            var walksDomain = await _walkRepository.GetAllAsync();

            // Convert domain walks to DTO Walks
            var walksDTO = _mapper.Map<List<Domain.Dtos.Walk>>(walksDomain);

            // Return response
            return Ok(walksDTO);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute]Guid id, [FromBody] Domain.Requests.UpdateWalk updateWalk)
        {
            // Convert DTO to Domain object
            var walkDomain = new Domain.Models.Walk
            {
                Length = updateWalk.Length,
                Name = updateWalk.Name,
                RegionId = updateWalk.RegionId,
                WalkDifficultyId = updateWalk.WalkDifficultyId
            };

            // Pass details to Repository - Get Domain object in response (or null)
            walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);

            // Handle Null (not found)
            if (walkDomain == null)
            {
                return NotFound();
            }

            // Convert back Domain to DTO
            var walkDTO = new Domain.Dtos.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // Return Response
            return Ok(walkDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // call Repository to delete walk
            var walkDomain = await _walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = _mapper.Map<Domain.Dtos.Walk>(walkDomain);

            return Ok(walkDTO);
        }
    }
}
