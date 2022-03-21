using System;

namespace Trainingsplanner.Postgres.Data.Models
{
    public class TrainingsModuleFollow
    {
        public int Id { get; set; }

        public int TrainingsModuleId { get; set; }
        public virtual TrainingsModule TrainingsModule { get; set; }

        public string UserId{ get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updatet { get; set; }

    }
}
