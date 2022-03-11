using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trainingsplanner.Postgres.Data;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.DataAccess.Implementation
{
    public class TrainingsModuleRepository : ITrainingsModuleRepository
    {
        private readonly ApplicationDbContext TrainingsContext;
        public TrainingsModuleRepository(ApplicationDbContext trainingsContext)
        {
            TrainingsContext = trainingsContext;
        }

        public async Task<List<TrainingsModule>> ReadAllTrainingsModule()
        {
            List<TrainingsModule> list = TrainingsContext.TrainingsModules.ToList();
            return list;
        }

        public async Task<List<TrainingsExercise>> ReadAllExercixesByModuleId(int id)
        {
            List<TrainingsExercise> exercises = TrainingsContext.TrainingsModulesTrainingsExercises
                .Where(x => x.TrainingsModuleId == id).Select(y => y.TrainingsExercise).ToList();

            return exercises;
        }

        public async Task<TrainingsModule> CreateTrainingsModule(TrainingsModule trainingsModule)
        {
            trainingsModule.Created = DateTime.UtcNow;
            trainingsModule.Updated = null;
            var entity = await TrainingsContext.TrainingsModules.AddAsync(trainingsModule);

            await TrainingsContext.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<TrainingsModuleTrainingsExercise> AddExerciseToModule(TrainingsModuleTrainingsExercise trainingsModuleTrainingsExercise)
        {
            if (trainingsModuleTrainingsExercise == null)
            {
                throw new ArgumentNullException();
            }

            var appointmentmodule = TrainingsContext.TrainingsModulesTrainingsExercises.Add(trainingsModuleTrainingsExercise);

            await TrainingsContext.SaveChangesAsync();

            return appointmentmodule.Entity;
        }

        public async Task<TrainingsModuleTrainingsModuleTag> AddTagToModule(TrainingsModuleTrainingsModuleTag trainingsModuleTrainingsModuleTag)
        {
            if (trainingsModuleTrainingsModuleTag == null)
            {
                throw new ArgumentNullException();
            }

            var ret = TrainingsContext.TrainingsModulesTrainingsModuleTags.Add(trainingsModuleTrainingsModuleTag);

            await TrainingsContext.SaveChangesAsync();

            return ret.Entity;
        }

        public async Task<TrainingsModule> UpdateTrainingsModule(TrainingsModule trainingsModule)
        {
            if (trainingsModule == null)
            {
                throw new ArgumentNullException();
            }

            trainingsModule.Updated = DateTime.UtcNow;
            var entity = TrainingsContext.TrainingsModules.Update(trainingsModule);
            await TrainingsContext.SaveChangesAsync();

            return entity.Entity;
        }
        public async Task<TrainingsModule> DeleteTrainingsModule(TrainingsModule trainingsModule)
        {
            if (trainingsModule == null)
            {
                throw new ArgumentNullException();
            }

            var entity = TrainingsContext.TrainingsModules.Remove(trainingsModule);
            var moduleExercises = TrainingsContext.TrainingsModulesTrainingsExercises.Where(x => x.TrainingsModuleId == trainingsModule.Id).ToList();

            foreach (var item in moduleExercises)
            {
                TrainingsContext.TrainingsModulesTrainingsExercises.Remove(item);
            }

            await TrainingsContext.SaveChangesAsync();

            return entity.Entity;
        }

        public async Task<TrainingsModuleTrainingsExercise> DeleteTrainingsModuleTrainingsExercise(int moduleId, int exerciseId)
        {
            if (moduleId < 0)
            {
                throw new ArgumentNullException();
            }
            if (exerciseId < 0)
            {
                throw new ArgumentNullException();
            }

            var moduleExercise = await TrainingsContext.TrainingsModulesTrainingsExercises
                .Where(x => x.TrainingsModuleId == moduleId).Where(y => y.TrainingsExerciesId == exerciseId).FirstOrDefaultAsync();

            var moduleExerciseDeleted = TrainingsContext.TrainingsModulesTrainingsExercises.Remove(moduleExercise);

            await TrainingsContext.SaveChangesAsync();

            return moduleExerciseDeleted.Entity;
        }

        public async Task<TrainingsModuleTrainingsModuleTag> DeleteTagFromModule(int moduleId, int tagId)
        {
            if (moduleId < 0)
            {
                throw new ArgumentNullException();
            }
            if (tagId < 0)
            {
                throw new ArgumentNullException();
            }

            var moduleTag = await TrainingsContext.TrainingsModulesTrainingsModuleTags
                .Where(x => x.TrainingsModuleId == moduleId).Where(y => y.TrainingsModuleTagId == tagId).FirstOrDefaultAsync();

            var moduleTagDeleted = TrainingsContext.TrainingsModulesTrainingsModuleTags.Remove(moduleTag);

            await TrainingsContext.SaveChangesAsync();

            return moduleTagDeleted.Entity;
        }


        public async Task<TrainingsModule> ReadTrainingsModuleById(int trainingsModuleId)
        {
            var entity = await TrainingsContext.TrainingsModules.FindAsync(trainingsModuleId);
            if (entity == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            return entity;
        }
        public async Task<List<TrainingsModule>> ReadTrainingsModulesByAppointmentId(int trainingsId)
        {
            List<TrainingsModule> list = await TrainingsContext.TrainingsAppointmentsTrainingsModules
                .Include(tatm => tatm.TrainingsModule)
                .Where(x => x.TrainingsModuleId == trainingsId)
                .Select(tatm => tatm.TrainingsModule)
                .ToListAsync();

            return list;
        }

        public async Task<List<TrainingsModule>> ReadTrainingsModulesByUserId(string userId)
        {
            return await TrainingsContext.TrainingsModules
                .Include(tm => tm.TrainingsModulesTrainingsModuleTags)
                .ThenInclude(t => t.TrainingsModuleTag)
                .Include(tm => tm.TrainingsModulesTrainingsExercises)
                .ThenInclude(tmte => tmte.TrainingsExercise)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<TrainingsModuleTag>> ReadAllTagsByModuleId(int moduleId)
        {
            return await TrainingsContext.TrainingsModulesTrainingsModuleTags
                .Where(x => x.TrainingsModuleId == moduleId)
                .Select(y => y.TrainingsModuleTag)
                .ToListAsync();
        }
    }
}