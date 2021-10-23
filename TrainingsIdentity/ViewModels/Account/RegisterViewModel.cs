using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public bool IsTrainer { get; set; }
    }
}