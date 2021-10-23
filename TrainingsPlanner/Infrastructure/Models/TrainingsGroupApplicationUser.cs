using System;

namespace TrainingsPlanner.Infrastructure.Models
{
    public class TrainingsGroupApplicationUser
    {
        public int TrainingsGroupId { get; set; }
        public TrainingsGroup TrainingsGroup { get; set; }
        public string ApplicationUserId { get; set; }
        
        public bool isTrainer { get; set; }
        public DateTime Created {get; set; }
        public DateTime? Updated { get; set; }
    }
}