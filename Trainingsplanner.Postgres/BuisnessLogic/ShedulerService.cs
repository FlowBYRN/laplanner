using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.ViewModels;
using Trainingsplanner.Postgres.ViewModels.Custom;

namespace Trainingsplanner.Postgres.BuisnessLogic
{
    internal sealed class ShedulerService : IShedulerService
    {
        private ITrainigsAppointmentRepository TrainigsAppointmentRepository { get; set; }

        public ShedulerService(ITrainigsAppointmentRepository trainigsAppointmentRepository)
        {
            TrainigsAppointmentRepository = trainigsAppointmentRepository;
        }
        public async Task<List<CalenderAppointmentDto>> ReadAllAppointmentsForCalender(int groupId, DateTime Startdate, DateTime Enddate)
        {
            var appointments = await TrainigsAppointmentRepository.ReadAllAppointmentsForCalender(groupId, Startdate, Enddate);
            var result = appointments.Select(appointment => new CalenderAppointmentDto()
            {
                Title = appointment.Title,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Id = appointment.Id,
                Modulelist = appointment.TrainingsAppointmentsTrainingsModules
                    .Select(tatm => tatm.TrainingsModule.Title)
                    .Aggregate("", (acc, title) => acc + $" - {title}<br>")
            }).ToList();

            return result;
        }

        public Task<int> SheduleTrainings(WeekDto week)
        {
            throw new System.NotImplementedException();
        }
    }
}
