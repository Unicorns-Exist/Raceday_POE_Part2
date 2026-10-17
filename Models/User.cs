using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Raceday_POE2.Models
{
    /// <summary>
    /// Represents a registered RaceDay user.
    /// A user can have the role Organiser or Participant.
    /// </summary>
    public class User
    {
        // Primary key: uniquely identifies each user.
        // EF Core recognises UserID as the primary key by convention.
        public int UserID { get; set; }

        // The user's first name is required.
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        // The user's surname is required.
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        // Each user must register with a valid email address.
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        // Stores the HASH of the password, never the original password.
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Allowed roles: Organiser or Participant.
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = string.Empty;

        // Navigation property: events created by this organiser.
        // One organiser can create multiple events.
        public ICollection<Event> OrganisedEvents { get; set; }
            = new List<Event>();

        // Navigation property: enrolments belonging to this participant.
        public ICollection<Enrolment> Enrolments { get; set; }
            = new List<Enrolment>();
    }
}
