using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingsPlanner.Infrastructure;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.DataAccess.Implementation
{
    public class TrainingsGroupUserRepository : ITrainingsGroupUserRepository
    {
        private readonly TrainingDbContext _context;

        public TrainingsGroupUserRepository(TrainingDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> CreateNewUserForGroup(TrainingsGroupApplicationUser trainingsGroupApplicationUser)
        {
            trainingsGroupApplicationUser.Created = DateTime.UtcNow;
            trainingsGroupApplicationUser.isTrainer = false;
            
            await _context.TrainingsGroupsApplicationUsers.AddAsync(trainingsGroupApplicationUser);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteMemberForGroup(int trainingsGroupId, string userId)
        {
            var entry = await _context.TrainingsGroupsApplicationUsers.Where(tgu => tgu.TrainingsGroupId == trainingsGroupId && tgu.ApplicationUserId == userId).FirstOrDefaultAsync();
            
            var entity = _context.TrainingsGroupsApplicationUsers.Remove(entry);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CreateNewTrainerForGroup(TrainingsGroupApplicationUser trainingsGroupApplicationUser)
        {
            trainingsGroupApplicationUser.Created = DateTime.UtcNow;
            trainingsGroupApplicationUser.isTrainer = true;
            
            await _context.TrainingsGroupsApplicationUsers.AddAsync(trainingsGroupApplicationUser);
            return await _context.SaveChangesAsync();
        }
        
    }
}