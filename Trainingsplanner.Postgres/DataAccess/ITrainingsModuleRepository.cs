using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;
using System.Collections.Generic;

namespace Trainingsplanner.Postgres.DataAccess
{
    public interface ITrainingsModuleRepository
    {

        Task<TrainingsModule> CreateTrainingsModule(TrainingsModule trainingsModule);
        Task<TrainingsModuleTrainingsExercise> AddExerciseToModule(TrainingsModuleTrainingsExercise trainingsModuleTrainingsExercise);
        Task<TrainingsModuleTrainingsModuleTag> AddTagToModule(TrainingsModuleTrainingsModuleTag trainingsModuleTrainingsModuleTag);
        Task<TrainingsModule> UpdateTrainingsModule(TrainingsModule trainingsModule);
        Task<TrainingsModule> DeleteTrainingsModule(TrainingsModule trainingsModule);
        Task<TrainingsModuleTrainingsExercise> DeleteTrainingsModuleTrainingsExercise(int moduleId, int exerciseId);
        Task<TrainingsModuleTrainingsModuleTag> DeleteTagFromModule(int moduleId, int tagId);
        Task<List<TrainingsModuleTag>> ReadAllTagsByModuleId(int moduleId);
        Task<TrainingsModule> ReadTrainingsModuleById(int trainingsModuleId);
        Task<List<TrainingsModule>> ReadTrainingsModulesByAppointmentId(int trainingsId);
        Task<List<TrainingsModule>> ReadTrainingsModulesByUserId(string userId);
        Task<List<TrainingsModule>> ReadAllTrainingsModule();
        Task<List<TrainingsExercise>> ReadAllExercixesByModuleId(int id);


    }
}