using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trainingsplanner.Postgres.Data;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.DataAccess.Implementation
{
    public class TrainingsGroupRepository : ITrainingsGroupRepository
    {
        private ApplicationDbContext Context { get; set; }

        public TrainingsGroupRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<List<TrainingsGroup>> ReadAllGroups()
        {
            return await Context.TrainingsGroups.Include(tg => tg.TrainingsGroupsApplicationUsers).ToListAsync();
        }

        public async Task<TrainingsGroup> ReadGroupById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException();
            }

            var trainingsgroup = await Context.TrainingsGroups
                .Include(tg => tg.TrainingsGroupsApplicationUsers)
                .Where(tg => tg.Id == id)
                .FirstOrDefaultAsync();

            return trainingsgroup;
        }

        public async Task<List<TrainingsAppointment>> ReadAppointmentsByGroupId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException();
            }

            var trainingsAppointments = await Context.TrainingsAppointments.Where(x => x.TrainingsGroupId == id).ToListAsync();

            if (trainingsAppointments.Count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            return trainingsAppointments;
        }

        public async Task<TrainingsGroup> CreateGroup(TrainingsGroup trainingsGroup)
        {
            if (trainingsGroup == null)
            {
                throw new ArgumentNullException();
            }
            trainingsGroup.Created = DateTime.UtcNow;
            var group = Context.TrainingsGroups.Add(trainingsGroup);

            await Context.SaveChangesAsync();

            return group.Entity;
        }

        public async Task<TrainingsGroup> UpdateGroup(TrainingsGroup trainingsGroup)
        {
            if (trainingsGroup == null)
            {
                throw new ArgumentNullException();
            }

            trainingsGroup.Updated = DateTime.Now;

            Context.TrainingsGroups.Update(trainingsGroup).State = EntityState.Modified;
            Context.TrainingsGroups.Update(trainingsGroup).Property(x => x.Created).IsModified = false;

            await Context.SaveChangesAsync();

            var group = await Context.TrainingsGroups.FindAsync(trainingsGroup.Id);

            return group;
        }

        public async Task<TrainingsGroup> DeleteGroup(TrainingsGroup trainingsGroup)
        {
            if (trainingsGroup == null)
            {
                throw new ArgumentNullException();
            }

            var group = Context.TrainingsGroups.Remove(trainingsGroup);

            await Context.SaveChangesAsync();

            return group.Entity;
        }

        public async Task<List<TrainingsGroupApplicationUser>> ReadTrainerIdsByGroup(int groupId)
        {
            return await Context.TrainingsGroupsApplicationUsers.Where(x => x.TrainingsGroupId == groupId && x.isTrainer).ToListAsync();
        }
    }
}
