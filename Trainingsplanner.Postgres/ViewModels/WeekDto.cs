using System;

namespace Trainingsplanner.Postgres.ViewModels
{
    public class WeekDto
    {
        public TrainingsDayDto Monday { get; set; }
        public TrainingsDayDto Tuesday { get; set; }
        public TrainingsDayDto Wednesday { get; set; }
        public TrainingsDayDto Thursday { get; set; }
        public TrainingsDayDto Friday { get; set; }
        public TrainingsDayDto Saturday { get; set; }
        public TrainingsDayDto Sunday { get; set; }
    }

    public class TrainingsDayDto
    {
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public bool IsToday { get; set; }
    }
}
