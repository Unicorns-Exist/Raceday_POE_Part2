using System.ComponentModel.DataAnnotations;

namespace Raceday_POE2.Models
{
    /// <summary>
    /// Represents a reusable category that can be offered
    /// at one or more RaceDay events.
    /// </summary>
    public class Category
    {
        // Primary key for the category.
        public int CategoryID { get; set; }

        // Example: Senior, Under 20, 10 km or Half Marathon.
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        // Optional distance associated with this category.
        // Nullable because an age category may not have
        // a specific distance.
        public decimal? DistanceKm { get; set; }

        // Identifies the category type: Age or Distance.
        [Required]
        [StringLength(20)]
        public string CategoryType { get; set; } = string.Empty;

        // Navigation property: events that offer this category.
        public ICollection<EventCategory> EventCategories { get; set; }
            = new List<EventCategory>();
    }
}