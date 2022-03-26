﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.ViewModels
{
    public class TrainingsAppointmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public int TrainingsGroupId { get; set; }

        public virtual TrainingsGroup TrainingsGroup { get; set; }

        public virtual ICollection<TrainingsAppointmentTrainingsModule> TrainingsAppointmentsTrainingsModules { get; set; } = new List<TrainingsAppointmentTrainingsModule>();

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

    }
}
