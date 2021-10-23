using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.DataAccess
{
    public interface ITrainingsExerciseRepository
    {
        Task<TrainingsExercise> CreateTrainingsExercise(TrainingsExercise trainingsExercise);
        Task<List<TrainingsExercise>> ReadAllTrainingsExercises();
        Task<TrainingsExercise> ReadTrainingsExerciseById(int id);
        Task<List<TrainingsExercise>> ReadTrainingsExercisesByTrainingsModuleId(int trainingsModuleId);
        Task<TrainingsExercise> UpdateTrainingsExercise(TrainingsExercise trainingsExercise);
        Task<TrainingsExercise> DeleteTrainingsExercise(TrainingsExercise trainingsExercise);
    }
}
