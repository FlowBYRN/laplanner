using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.ViewModels
{
    public class TrainingsModuleTagDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<TrainingsModuleTrainingsModuleTag>
            TrainingsModulesTrainingsModuleTags
        { get; set; } = new List<TrainingsModuleTrainingsModuleTag>();
    }
}
