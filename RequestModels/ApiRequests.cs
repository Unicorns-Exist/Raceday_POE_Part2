using System.ComponentModel.DataAnnotations;
namespace Raceday_POE2.Models;

public class UpdateProfileRequest
{
    [Required, StringLength(50)] public string FirstName { get; set; } = string.Empty;
    [Required, StringLength(50)] public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = string.Empty;
}

public class EventWriteRequest
{
    [Required, StringLength(100)] public string EventName { get; set; } = string.Empty;
    [Required, StringLength(1000)] public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    [Required, StringLength(200)] public string Location { get; set; } = string.Empty;
    [Range(typeof(decimal), "0.01", "9999.99")] public decimal DistanceKm { get; set; }
    [Required, RegularExpression("^(Run|Walk|Cycle)$")] public string EventType { get; set; } = string.Empty;
}

public class CategoryWriteRequest
{
    [Required, StringLength(100)] public string CategoryName { get; set; } = string.Empty;
    public decimal? DistanceKm { get; set; }
    [Required, RegularExpression("^(Age|Distance)$")] public string CategoryType { get; set; } = string.Empty;
}

public class EnrolmentCreateRequest
{
    [Range(1, int.MaxValue)] public int EventCategoryID { get; set; }
}

public class ResultCreateRequest
{
    [Range(1, int.MaxValue)] public int EnrolmentID { get; set; }
    public TimeSpan FinishTime { get; set; }
    [Range(1, int.MaxValue)] public int FinishingPosition { get; set; }
}
