using System.Collections.Generic;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.DataAccess
{
    public interface ITrainingsGroupUserRepository
    {
        Task<int> CreateNewUserForGroup(TrainingsGroupApplicationUser trainingsGroupApplicationUser);
        Task<int> DeleteMemberForGroup(int trainingsGroupId, string userId);
        Task<int> CreateNewTrainerForGroup(TrainingsGroupApplicationUser trainingsGroupApplicationUser);
    }
}