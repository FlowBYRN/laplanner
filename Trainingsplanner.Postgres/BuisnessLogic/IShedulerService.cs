using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.ViewModels;
using Trainingsplanner.Postgres.ViewModels.Custom;

namespace Trainingsplanner.Postgres.BuisnessLogic
{
    public interface IShedulerService
    {
        Task<int> SheduleTrainings(WeekDto week);
        Task<List<CalenderAppointmentDto>> ReadAllAppointmentsForCalender(int groupId, DateTime Startdate, DateTime Enddate);
    }
}
