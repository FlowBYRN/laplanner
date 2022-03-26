using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.ViewModels
{
    public class TrainingsModuleTagDto
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
