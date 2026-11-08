using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday_POE2.Data;
using Raceday_POE2.Models;

namespace Raceday_POE2.Controllers;

[ApiController]
[Route("api/results")]
[Authorize(Roles = "Organiser,Participant")]
public class ResultsController : ControllerBase
{
    private readonly RaceDayContext _context;
    public ResultsController(RaceDayContext context) => _context = context;

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateResult(ResultCreateRequest request)
    {
        var enrolment = await _context.Enrolments.FindAsync(request.EnrolmentID);
        if (enrolment == null) return NotFound(new { message = "Enrolment not found." });
        if (enrolment.Status != "Active") return Conflict(new { message = "A result cannot be recorded for a cancelled enrolment." });
        if (await _context.Results.AnyAsync(r => r.EnrolmentID == request.EnrolmentID)) return Conflict(new { message = "A result already exists for this enrolment." });
        var item = new Result { EnrolmentID = request.EnrolmentID, FinishTime = request.FinishTime, FinishingPosition = request.FinishingPosition };
        _context.Results.Add(item);
        await _context.SaveChangesAsync();
        return Created("/api/results/my", new { item.ResultID, item.EnrolmentID, item.FinishTime, item.FinishingPosition });
    }

    [HttpGet("my")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> GetMyResults()
    {
        var participantId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var items = await _context.Results.AsNoTracking().Where(r => r.Enrolment!.ParticipantID == participantId)
            .Select(r => new { r.ResultID, r.EnrolmentID, r.FinishTime, r.FinishingPosition, EventName = r.Enrolment!.EventCategory!.Event!.EventName, CategoryName = r.Enrolment.EventCategory.Category!.CategoryName })
            .ToListAsync();
        return Ok(items);
    }
}
