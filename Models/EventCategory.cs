
using System.ComponentModel.DataAnnotations;

namespace Raceday_POE2.Models
{
    /// <summary>
    /// Links an Event to a Category.
    /// It also stores details that apply to that category
    /// at that particular event.
    /// </summary>
    public class EventCategory
    {
        // Primary key for this event-category combination.
        public int EventCategoryID { get; set; }

        // Foreign key: the event offering the category.
        public int EventID { get; set; }

        // Foreign key: the category being offered.
        public int CategoryID { get; set; }

        // The entry fee for this category at this event.
        public decimal EntryFee { get; set; }

        // Optional maximum number of participants.
        public int? MaxParticipants { get; set; }

        // Navigation property to the event.
        public Event? Event { get; set; }

        // Navigation property to the category.
        public Category? Category { get; set; }

        // Participants who enrol in this event-category combination.
        public ICollection<Enrolment> Enrolments { get; set; }
            = new List<Enrolment>();
    }
}
