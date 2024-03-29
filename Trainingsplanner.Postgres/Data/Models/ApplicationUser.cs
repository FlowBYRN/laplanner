﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trainingsplanner.Postgres.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalDataAttribute]
        public string LastName { get; set; }
        [PersonalDataAttribute]
        public string FirstName { get; set; }

        public virtual ICollection<TrainingsModuleFollow> Follows { get; set; } = new List<TrainingsModuleFollow>();
    }
}