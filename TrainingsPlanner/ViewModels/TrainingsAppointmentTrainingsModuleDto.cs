using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.ViewModels
{
    public class TrainingsAppointmentTrainingsModuleDto
    {
        public int TrainingsModuleId { get; set; }
        public virtual TrainingsModule TrainingsModule { get; set; }
        public int TrainingsAppointmentId { get; set; }
        public virtual TrainingsAppointment TrainingsAppointment { get; set; }
    }
}
