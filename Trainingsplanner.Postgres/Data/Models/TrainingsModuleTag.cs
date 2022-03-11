using System;
using System.Collections.Generic;

namespace Trainingsplanner.Postgres.Data.Models
{
    public class TrainingsModuleTag
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<TrainingsModuleTrainingsModuleTag>
            TrainingsModulesTrainingsModuleTags
        { get; set; } = new List<TrainingsModuleTrainingsModuleTag>();

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}