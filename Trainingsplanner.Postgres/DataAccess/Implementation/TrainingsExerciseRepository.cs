using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.DataAccess.Implementation
{
    public class TrainingsExerciseRepository : ITrainingsExerciseRepository
    {
        private readonly ApplicationDbContext TrainingsContext;

        public TrainingsExerciseRepository(ApplicationDbContext trainingsContext)
        {
            TrainingsContext = trainingsContext;
        }

        public async Task<TrainingsExercise> CreateTrainingsExercise(TrainingsExercise trainingsExercise)
        {
            trainingsExercise.Created = DateTime.UtcNow;
            trainingsExercise.Updated = null;
            var entity = await TrainingsContext.TrainingsExercises.AddAsync(trainingsExercise);
            await TrainingsContext.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<TrainingsExercise> DeleteTrainingsExercise(TrainingsExercise trainingsExercise)
        {
            var exercise = TrainingsContext.TrainingsExercises.Remove(trainingsExercise);
            await TrainingsContext.SaveChangesAsync();
            return exercise.Entity;
        }

        public async Task<List<TrainingsExercise>> ReadAllTrainingsExercises()
        {
            var trainingsExercises = await TrainingsContext.TrainingsExercises.ToListAsync();
            return trainingsExercises;
        }

        public async Task<TrainingsExercise> ReadTrainingsExerciseById(int trainingsExerciseId)
        {
            TrainingsExercise trainingsExercise = await TrainingsContext.TrainingsExercises.FindAsync(trainingsExerciseId);
            return trainingsExercise;
        }

        public async Task<List<TrainingsExercise>> ReadTrainingsExercisesByTrainingsModuleId(int trainingsModuleId)
        {
            List<TrainingsExercise> trainingsExercises = await TrainingsContext.TrainingsModulesTrainingsExercises
                .Include(tmte => tmte.TrainingsExercise)
                .Where(x => x.TrainingsModuleId == trainingsModuleId)
                .Select(tmte => tmte.TrainingsExercise)
                .ToListAsync();
            return trainingsExercises;
        }

        public async Task<TrainingsExercise> UpdateTrainingsExercise(TrainingsExercise trainingsExercise)
        {
            if (trainingsExercise == null)
            {
                throw new ArgumentNullException();
            }
            trainingsExercise.Updated = DateTime.UtcNow;
            var exercise = TrainingsContext.TrainingsExercises.Update(trainingsExercise);
            await TrainingsContext.SaveChangesAsync();

            return exercise.Entity;
        }
    }


}
