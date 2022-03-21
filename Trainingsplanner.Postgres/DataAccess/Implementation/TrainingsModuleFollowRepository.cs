using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.DataAccess.Implementation
{
    internal sealed class TrainingsModuleFollowRepository : ITrainingsModuleFollowRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainingsModuleFollowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async Task<TrainingsModuleFollow> ITrainingsModuleFollowRepository.Follow(TrainingsModuleFollow trainingsModuleFollow)
        {
            trainingsModuleFollow.Created = DateTime.UtcNow;
            trainingsModuleFollow.Updatet = null;

            await _context.TrainingsModuleFollows.AddAsync(trainingsModuleFollow);
            await _context.SaveChangesAsync();
            return trainingsModuleFollow;
        }

        async Task<List<TrainingsModule>> ITrainingsModuleFollowRepository.ReadFollowedTrainingsModules(string userId)
        {
            var entries = _context.TrainingsModuleFollows
                .Include(tmf => tmf.TrainingsModule)
                .Where(tmf => tmf.UserId == userId)
                .Select(tmf => tmf.TrainingsModule)
                .ToList();

            return entries;
        }

        async Task<List<ApplicationUser>> ITrainingsModuleFollowRepository.ReadTrainingsModuleFollowers(int trainingsModuleId)
        {
            var entries = _context.TrainingsModuleFollows
                .Include(tmf => tmf.User)
                .Where(tmf => tmf.TrainingsModuleId == trainingsModuleId)
                .Select(tmf => tmf.User)
                .ToList();

            foreach (var entry in entries)
            {
                entry.PasswordHash = null;
            }

            return entries;
        }

        async Task<TrainingsModuleFollow> ITrainingsModuleFollowRepository.UnFollow(TrainingsModuleFollow trainingsModuleFollow)
        {
            var entry = await _context.TrainingsModuleFollows.Where(tgu => tgu.UserId == trainingsModuleFollow.UserId && tgu.TrainingsModuleId == trainingsModuleFollow.TrainingsModuleId).FirstOrDefaultAsync();

            var entity = _context.TrainingsModuleFollows.Remove(entry);
            await _context.SaveChangesAsync();
            return entry;
        }
    }
}
