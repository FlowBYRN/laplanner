using System;

namespace Trainingsplanner.Postgres.ViewModels.Custom
{
    public class CalenderAppointmentDto
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Modulelist { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
