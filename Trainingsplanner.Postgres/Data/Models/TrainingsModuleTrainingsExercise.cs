using System;

namespace Trainingsplanner.Postgres.Data.Models
{
    public class TrainingsModuleTrainingsExercise
    {
        public int TrainingsModuleId { get; set; }
        public virtual TrainingsModule TrainingsModule { get; set; }
        public int TrainingsExerciesId { get; set; }
        public virtual TrainingsExercise TrainingsExercise { get; set; }

        public int Position { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}