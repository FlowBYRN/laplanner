using System.Collections.Generic;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.DataAccess
{
    public interface ITrainingsModuleFollowRepository
    {
        Task<TrainingsModuleFollow> Follow(TrainingsModuleFollow trainingsModuleFollow);
        Task<TrainingsModuleFollow> UnFollow(TrainingsModuleFollow trainingsModuleFollow);
        Task<List<TrainingsModule>> ReadFollowedTrainingsModules(string userId);
        Task<List<ApplicationUser>> ReadTrainingsModuleFollowers(int trainingsModuleId);
        Task<TrainingsModuleFollow> ReadTrainingsModuleFollow(TrainingsModuleFollow trainingsModuleFollow);

    }
}
