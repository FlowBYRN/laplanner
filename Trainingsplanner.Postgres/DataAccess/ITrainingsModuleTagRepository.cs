using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.DataAccess
{
    public interface ITrainingsModuleTagRepository
    {
        Task<List<TrainingsModuleTag>> ReadAllTags();

        Task<TrainingsModuleTag> ReadTagById(int id);

        Task<TrainingsModuleTag> ReadTagByName(string name);

        Task<TrainingsModuleTag> InsertTag(TrainingsModuleTag trainingsModuleTag);

        Task<TrainingsModuleTag> UpdateTag(TrainingsModuleTag trainingsModuleTag);

        Task<TrainingsModuleTag> DeleteTag(TrainingsModuleTag trainingsModuleTag);
    }
}
