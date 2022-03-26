using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.ViewModels
{
    public class TrainingsGroupDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TrainingsGroupApplicationUser> TrainingsGroupsApplicationUsers { get; set; } = new List<TrainingsGroupApplicationUser>();
        public virtual ICollection<TrainingsAppointment> TrainingsAppointments { get; set; } = new List<TrainingsAppointment>();

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
