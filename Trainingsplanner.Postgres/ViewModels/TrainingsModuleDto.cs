using System;
using System.Collections.Generic;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.ViewModels
{
    public class TrainingsModuleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public bool isFollowed { get; set; }
        public TrainingsDifficulty Difficulty { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<TrainingsModuleTrainingsExercise> TrainingsModulesTrainingsExercises { get; set; } =
            new List<TrainingsModuleTrainingsExercise>();

        public virtual ICollection<TrainingsModuleTrainingsModuleTag> TrainingsModulesTrainingsModuleTags { get; set; } =
            new List<TrainingsModuleTrainingsModuleTag>();

        public virtual ICollection<TrainingsAppointmentTrainingsModule> TrainingsAppointmentsTrainingsModules { get; set; } =
            new List<TrainingsAppointmentTrainingsModule>();

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}