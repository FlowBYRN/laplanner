using System;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.ViewModels
{
    public class TrainingsModuleFollowDto
    {
        public int Id { get; set; }

        public int TrainingsModuleId { get; set; }
        public virtual TrainingsModuleDto TrainingsModule { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updatet { get; set; }
    }
}
