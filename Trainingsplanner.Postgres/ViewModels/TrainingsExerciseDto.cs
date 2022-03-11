using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.ViewModels
{
    public class TrainingsExerciseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Repetitions { get; set; }

        public virtual ICollection<TrainingsModuleTrainingsExercise> TrainingsModulesTrainingsExercises { get; set; } =
            new List<TrainingsModuleTrainingsExercise>();
    }
}
