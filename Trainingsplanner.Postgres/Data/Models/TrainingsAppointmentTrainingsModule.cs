using System;

namespace Trainingsplanner.Postgres.Data.Models
{
    public class TrainingsAppointmentTrainingsModule
    {
        public int TrainingsModuleId { get; set; }
        public virtual TrainingsModule TrainingsModule { get; set; }
        public int TrainingsAppointmentId { get; set; }
        public virtual TrainingsAppointment TrainingsAppointment { get; set; }

        public int OrderId{ get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}