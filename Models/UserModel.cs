using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Backoffice_ANCFCC.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "The Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The Name is required.")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "The LastName is required.")]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "The Role is required. Choose Admin or agent. ")]
        public string Role { get; set; }

        [Required(ErrorMessage = "The email is required")]
        [EmailAddress(ErrorMessage = "Email not valid")]
        [Compare("Email", ErrorMessage = "The email must be different to the password")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The password is required.")]
        [MinLength(6, ErrorMessage = "The password is not strong enough!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "The CheckPassword is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string CheckPassword { get; set; }


    }
}
