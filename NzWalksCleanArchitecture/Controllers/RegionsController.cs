using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NzWalksCleanArchitecture.Application.Interfaces;
using NzWalksCleanArchitecture.Domain.Requests;
using System.Data;

namespace NzWalksCleanArchitecture.Controllers
{
    [ApiController]

    [Route("[controller]")]
    public class RegionsController : Controller
    {
       
        private IRegionRepository _regionRepository;
        private IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        [HttpGet("{id:guid}")]
        [ActionName("GetRegionAsync")]

        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await _regionRepository.GetAsync(id);
            if (region == null) { return NotFound(); }
            var regionDto = _mapper.Map<Domain.Dtos.Region>(region);
            return Ok(regionDto);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();
            var regionsDto = _mapper.Map<List<Domain.Dtos.Region>>(regions);
            return Ok(regionsDto);

        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Domain.Requests.AddRegionRequest addRegion) 
        {
            // Validate The Request
            if (!ValidateAddRegionAsync(addRegion))
            {
                return BadRequest(ModelState);
            }
            // mAke a Dto to Model  request
            var region = new Domain.Models.Region()
            {
                Code = addRegion.Code,
                Area = addRegion.Area,
                Lat = addRegion.Lat,
                Long = addRegion.Long,
                Name = addRegion.Name,
                Population = addRegion.Population,
            };

            //Pass details above to the repository

            region = await _regionRepository.AddAsync(region);

            // Convert the method above back to a Dto

            var regionDto = new Domain.Dtos.Region
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long=region.Long,
                Population =  region.Population,
            };
            
            // 
            return CreatedAtAction(nameof(GetRegionAsync), new {id = regionDto.Id}, regionDto);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync (Guid id)
        {
            // Get Region from the database

            var region =await _regionRepository.DeleteAsync(id);

            // if null Return Notfound
            if (region == null)
            {
                return NotFound(); 
            }

            // Convert response back to a Dto

            var regionDto = new Domain.Dtos.Region
            {
                Id= region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
                
            };

            // Return response
            return Ok(regionDto);

        }

        [HttpPut("{id:guid}")]
        
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,[FromBody]Domain.Requests.UpdateRegionRequest
            updateRegion)
        {
            // Validate the incoming request
            if (!ValidateUpdateRegionAsync(updateRegion))
            {
                return BadRequest(ModelState);
            }

            //Convert Dto to Model 
            var region = new Domain.Models.Region()
            {
                Code = updateRegion.Code,
                Name = updateRegion.Name,
                Area = updateRegion.Area,
                Lat = updateRegion.Lat,
                Long = updateRegion.Long,
                Population = updateRegion.Population,
            };
            //Update Region using repository
            region = await _regionRepository.UpdateAsync(id,region);

            //if null returrn notfound

            if (region == null)
            {
                return NotFound();
            }

            // Else convert Model back to Dto
            var regionDto = new Domain.Dtos.Region
            {
                Id= region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,

            };

            // return ok response

            return Ok(regionDto);
        }

        #region Validatiom Methods

        private bool ValidateAddRegionAsync(Domain.Requests.AddRegionRequest addRegion)
        {
            if(addRegion == null)
            {
                ModelState.AddModelError(nameof(addRegion), $"AddRegion Data is Required");return false;
            }
            if (string.IsNullOrWhiteSpace(addRegion.Code))
            {
                ModelState.AddModelError(nameof(addRegion.Code),
                    $"{nameof(  addRegion.Code)} cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(addRegion.Name))
            {
                ModelState.AddModelError(nameof(addRegion.Name),
                    $"{nameof(addRegion.Name)} cannot be null or empty or white space.");
            }

            if (addRegion.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegion.Area),
                    $"{nameof(addRegion.Area)} cannot be less than or equal to zero.");
            }

            if (addRegion.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegion.Population),
                    $"{nameof(addRegion.Population)} cannot be less than zero.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUpdateRegionAsync(Domain.Requests.UpdateRegionRequest updateRegion)
        {
            if (updateRegion == null)
            {
                ModelState.AddModelError(nameof(updateRegion),
                    $"Add Region Data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateRegion.Code))
            {
                ModelState.AddModelError(nameof(updateRegion.Code),
                    $"{nameof(updateRegion.Code)} cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(updateRegion.Name))
            {
                ModelState.AddModelError(nameof(updateRegion.Name),
                    $"{nameof(updateRegion.Name)} cannot be null or empty or white space.");
            }

            if (updateRegion.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegion.Area),
                    $"{nameof(updateRegion.Area)} cannot be less than or equal to zero.");
            }

            if (updateRegion.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegion.Population),
                    $"{nameof(updateRegion.Population)} cannot be less than zero.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion

    }


}
