using System.ComponentModel.DataAnnotations;

namespace Backoffice_ANCFCC.Models
{
    public class ResetPasswordRequest
    {


        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        [MinLength(6, ErrorMessage = "Please enter at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        [MinLength(6, ErrorMessage = "Please enter at least 6 characters.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
