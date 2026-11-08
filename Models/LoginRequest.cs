
using System.ComponentModel.DataAnnotations;

namespace Raceday_POE2.Models
{
    /// <summary>
    /// Information supplied when a user logs in.
    /// </summary>
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }// end of class

}// end of namespace
