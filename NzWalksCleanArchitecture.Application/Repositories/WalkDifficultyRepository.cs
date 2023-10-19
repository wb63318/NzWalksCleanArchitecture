using NzWalksCleanArchitecture.Application.Interfaces;
using NzWalksCleanArchitecture.Domain.Models;
using NzWalksCleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NzWalksCleanArchitecture.Application.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _dbcontext;
        public WalkDifficultyRepository(NZWalksDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await _dbcontext.WalkDifficulty.AddAsync(walkDifficulty);
            await _dbcontext.SaveChangesAsync();
            return walkDifficulty;
        }
        // Use findAsync with if(var != null) and firstorDefaultasync with if(var == null)
        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await _dbcontext.WalkDifficulty.FindAsync(id);
            if (existingWalkDifficulty !=null) { _dbcontext.WalkDifficulty.Remove(existingWalkDifficulty);
                await _dbcontext.SaveChangesAsync();
                return existingWalkDifficulty;
            }
            return null;
            
            
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _dbcontext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await _dbcontext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await _dbcontext.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficulty == null)
            {
                return null;
            }

            existingWalkDifficulty.Code = walkDifficulty.Code;
            await _dbcontext.SaveChangesAsync();
            return existingWalkDifficulty;
        }
    }
}
