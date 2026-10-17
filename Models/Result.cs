using System.ComponentModel.DataAnnotations;

namespace Raceday_POE2.Models
{
    /// <summary>
    /// Represents the final result for an enrolment.
    /// </summary>
    public class Result
    {
        // Primary key for the result.
        public int ResultID { get; set; }

        // Foreign key: identifies the enrolment being scored.
        // A unique constraint will ensure one result per enrolment.
        public int EnrolmentID { get; set; }

        // The participant's elapsed finishing time.
        // TimeSpan represents a duration, such as 01:25:30.
        public TimeSpan FinishTime { get; set; }

        // The participant's finishing position, such as 1, 2 or 3.
        public int FinishingPosition { get; set; }

        // Navigation property back to the enrolment.
        public Enrolment? Enrolment { get; set; }
    }
}