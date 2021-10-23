using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.ViewModels
{
    public class TrainingsModuleTrainingsExerciseDto
    {
        public int TrainingsModuleId { get; set; }
        public virtual TrainingsModule TrainingsModule { get; set; }
        public int TrainingsExerciesId { get; set; }
        public virtual TrainingsExercise TrainingsExercise { get; set; }

        public int Position { get; set; }
    }
}
