using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday_POE2.Data;

namespace Raceday_POE2.Controllers;

[ApiController]
[Route("api/events/{eventId:int}/enrolments")]
[Authorize(Roles = "Organiser")]
public class EventEnrolmentsController : ControllerBase
{
    private readonly RaceDayContext _context;
    public EventEnrolmentsController(RaceDayContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetEventEnrolments(int eventId)
    {
        if (!await _context.Events.AnyAsync(e => e.EventID == eventId)) return NotFound(new { message = "Event not found." });
        var items = await _context.Enrolments.AsNoTracking()
            .Where(e => e.EventCategory!.EventID == eventId)
            .Select(e => new { e.EnrolmentID, e.ParticipantID, ParticipantName = e.Participant!.FirstName + " " + e.Participant.LastName, e.Participant.Email, e.EventCategoryID, CategoryName = e.EventCategory!.Category!.CategoryName, e.EnrolmentDate, e.Status })
            .ToListAsync();
        return Ok(items);
    }
}
