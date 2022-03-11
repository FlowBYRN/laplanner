using System;
using System.Collections.Generic;

namespace Trainingsplanner.Postgres.Data.Models
{
    public class TrainingsExercise
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Repetitions { get; set; }

        public virtual ICollection<TrainingsModuleTrainingsExercise> TrainingsModulesTrainingsExercises { get; set; } =
            new List<TrainingsModuleTrainingsExercise>();

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}