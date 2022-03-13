using System.ComponentModel.DataAnnotations;

namespace Trainingsplanner.Postgres.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsTrainer { get; set; }
    }
}