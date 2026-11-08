using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday_POE2.Data;
using Raceday_POE2.Models;

namespace Raceday_POE2.Controllers;

[ApiController]
[Route("api/enrolments")]
[Authorize(Roles = "Participant")]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayContext _context;
    public EnrolmentsController(RaceDayContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> CreateEnrolment(EnrolmentCreateRequest request)
    {
        var participantId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var eventCategory = await _context.EventCategories.FindAsync(request.EventCategoryID);
        if (eventCategory == null) return NotFound(new { message = "Event category not found." });
        if (eventCategory.MaxParticipants.HasValue)
        {
            var activeCount = await _context.Enrolments.CountAsync(e => e.EventCategoryID == request.EventCategoryID && e.Status == "Active");
            if (activeCount >= eventCategory.MaxParticipants.Value) return Conflict(new { message = "This category has reached its participant limit." });
        }
        var duplicate = await _context.Enrolments.AnyAsync(e => e.ParticipantID == participantId && e.EventCategoryID == request.EventCategoryID && e.Status == "Active");
        if (duplicate) return Conflict(new { message = "You are already enrolled in this category." });
        var item = new Enrolment { ParticipantID = participantId, EventCategoryID = request.EventCategoryID, Status = "Active" };
        _context.Enrolments.Add(item);
        await _context.SaveChangesAsync();
        return Created($"/api/enrolments/my", new { item.EnrolmentID, item.ParticipantID, item.EventCategoryID, item.EnrolmentDate, item.Status });
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyEnrolments()
    {
        var participantId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var items = await _context.Enrolments.AsNoTracking().Where(e => e.ParticipantID == participantId)
            .Select(e => new { e.EnrolmentID, e.EventCategoryID, e.EnrolmentDate, e.Status, EventName = e.EventCategory!.Event!.EventName, CategoryName = e.EventCategory.Category!.CategoryName })
            .ToListAsync();
        return Ok(items);
    }
}
