using System.ComponentModel.DataAnnotations;

namespace Backoffice_ANCFCC.Models
{
    public class CandidaturePersoDTO
    {
        [Required]
        public string Reference { get; set; }
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public string CIN { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Diplome { get; set; }
        [Required]
        public double? Note { get; set; }
        [Required]
        public int? AnneeExperience { get; set; }
        
    }
    public class CandidatureValidationDTO 
    {
        [Required]
        public string Reference { get; set; }
        public string TreatedByAgent { get; set; }
        public string TreatedByAdmin { get; set; }
        [Required]
        public string Status { get; set; }
        public string Message { get; set; }

        public DateTime? Date_de_Modification { get; set; }
    }
}
