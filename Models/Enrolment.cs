using System.ComponentModel.DataAnnotations;

namespace Raceday_POE2.Models
{
    /// <summary>
    /// Represents a Participant's entry into an event category.
    /// </summary>
    public class Enrolment
    {
        // Primary key for the enrolment.
        public int EnrolmentID { get; set; }

        // Foreign key: the Participant who entered.
        public int ParticipantID { get; set; }

        // Foreign key: the selected event-category combination.
        public int EventCategoryID { get; set; }

        // Date and time when the enrolment was recorded.
        public DateTime EnrolmentDate { get; set; }

        // Tracks whether the enrolment is Active or Cancelled.
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        // Navigation property to the Participant.
        public User? Participant { get; set; }

        // Navigation property to the selected event category.
        public EventCategory? EventCategory { get; set; }

        // A participant may not have a result yet.
        // The result is recorded after the event.
        public Result? Result { get; set; }
    }
}