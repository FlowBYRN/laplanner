using System;
using System.Collections.Generic;

namespace Trainingsplanner.Postgres.Data.Models
{
    public class TrainingsModule
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public TrainingsDifficulty Difficulty { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<TrainingsModuleTrainingsExercise> TrainingsModulesTrainingsExercises { get; set; } =
            new List<TrainingsModuleTrainingsExercise>();

        public virtual ICollection<TrainingsModuleTrainingsModuleTag> TrainingsModulesTrainingsModuleTags { get; set; } =
            new List<TrainingsModuleTrainingsModuleTag>();

        public virtual ICollection<TrainingsAppointmentTrainingsModule> TrainingsAppointmentsTrainingsModules { get; set; } =
            new List<TrainingsAppointmentTrainingsModule>();

        public virtual ICollection<TrainingsModuleFollow> Followers { get; set; } = new List<TrainingsModuleFollow>();

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }

    public enum TrainingsDifficulty
    {
        UNKNOWN = 0,
        EASY = 10,
        MEDIUM = 20,
        HARD = 30,
        IMPOSSIBLE = 40
    }
}