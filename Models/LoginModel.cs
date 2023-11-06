using System.ComponentModel.DataAnnotations;

namespace Backoffice_ANCFCC.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }




    }
}
