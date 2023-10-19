using Microsoft.EntityFrameworkCore;
using NzWalksCleanArchitecture.Application.Interfaces;
using NzWalksCleanArchitecture.Domain.Models;
using NzWalksCleanArchitecture.Infrastructure.Data;
//using NzWalksCleanArchitecture.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NzWalksCleanArchitecture.Application.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public RegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await dbContext.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            // check if region to be deletedd is available
            var region =await dbContext.Regions.FirstOrDefaultAsync(x=> x.Id == id);
            if(region == null) { return null; }

            // if region is found , proceed to delete reegion

            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();
            return region;

        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            // return the list of all available regions
            return await dbContext.Regions.ToListAsync();
        }

        public Task<Region> GetAsync(Guid id)
        {
            return dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            //check if the region to be updated is available
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            // return a error if the region is not found

            if(existingRegion == null)
            {
                return null;
            }
            //else update the fields below

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;

            await dbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
