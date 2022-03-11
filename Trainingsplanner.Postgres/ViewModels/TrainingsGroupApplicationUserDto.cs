using System;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.ViewModels
{
    public class TrainingsGroupApplicationUserDto
    {
        public int TrainingsGroupId { get; set; }
        public TrainingsGroup TrainingsGroup { get; set; }
        public string ApplicationUserId { get; set; }

        public bool isTrainer { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}