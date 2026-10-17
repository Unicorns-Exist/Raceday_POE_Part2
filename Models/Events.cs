using System.ComponentModel.DataAnnotations;

namespace Raceday_POE2.Models
{
    /// <summary>
    /// Represents a RaceDay event.
    /// Every event is created by an Organiser.
    /// </summary>
    public class Event
    {
        // Primary key for the event.
        public int EventID { get; set; }

        // Name displayed to people browsing events.
        [Required]
        [StringLength(100)]
        public string EventName { get; set; } = string.Empty;

        // Description explaining what the event involves.
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        // Date and time when the event takes place.
        public DateTime EventDate { get; set; }

        // Where the event takes place.
        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        // The event distance, measured in kilometres.
        public decimal DistanceKm { get; set; }

        // The event type: Run, Walk or Cycle.
        [Required]
        [StringLength(20)]
        public string EventType { get; set; } = string.Empty;

        // Foreign key: identifies the Organiser who created the event.
        public int OrganiserID { get; set; }

        // Navigation property to the event's organiser.
        public User? Organiser { get; set; }

        // Categories linked to this event through EventCategory.
        public ICollection<EventCategory> EventCategories { get; set; }
            = new List<EventCategory>();
    }
}
