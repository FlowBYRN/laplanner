using System;

namespace TrainingsPlanner.Infrastructure.Models
{
    public class TrainingsAppointmentTrainingsModule
    {
        public int TrainingsModuleId { get; set; }
        public virtual TrainingsModule TrainingsModule { get; set; }
        public int TrainingsAppointmentId { get; set; }
        public virtual TrainingsAppointment TrainingsAppointment { get; set; }
        
        //public int Position { get; set; }
        
        public DateTime Created {get; set; }
        public DateTime? Updated { get; set; }
    }
}