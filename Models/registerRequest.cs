using System.ComponentModel.DataAnnotations;

namespace Raceday_POE2.Models
{
    /// <summary>
    /// Information supplied when a new user registers.
    /// </summary>
    public class registerRequest
    {//start of class
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Organiser|Participant)$",
            ErrorMessage = "Role must be Organiser or Participant.")]
        public string Role { get; set; } = string.Empty;
    }// end of class

}

