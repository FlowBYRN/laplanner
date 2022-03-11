using System;

namespace Trainingsplanner.Postgres.Data.Models
{
    public class TrainingsModuleTrainingsModuleTag
    {
        public int TrainingsModuleId { get; set; }
        public TrainingsModule TrainingsModule { get; set; }
        public int TrainingsModuleTagId { get; set; }
        public TrainingsModuleTag TrainingsModuleTag { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}