using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.DataAccess
{
    public interface ITrainingsGroupRepository
    {

        Task<List<TrainingsGroup>> ReadAllGroups();

        Task<TrainingsGroup> ReadGroupById(int id);

        Task<List<TrainingsAppointment>> ReadAppointmentsByGroupId(int id);

        Task<TrainingsGroup> CreateGroup(TrainingsGroup trainingsGroup);

        Task<TrainingsGroup> UpdateGroup(TrainingsGroup trainingsGroup);

        Task<TrainingsGroup> DeleteGroup(TrainingsGroup trainingsGroup);
    }
}
