using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday_POE2.Data;
using Raceday_POE2.Models;

namespace Raceday_POE2.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly RaceDayContext _context;
    public EventsController(RaceDayContext context) => _context = context;

    [HttpGet]
    [Authorize(Roles = "Organiser,Participant")]
    public async Task<IActionResult> GetEvents() => Ok(await _context.Events.AsNoTracking()
        .Select(e => new { e.EventID, e.EventName, e.Description, e.EventDate, e.Location, e.DistanceKm, e.EventType, e.OrganiserID }).ToListAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Organiser,Participant")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var item = await _context.Events.AsNoTracking().Where(e => e.EventID == id)
            .Select(e => new { e.EventID, e.EventName, e.Description, e.EventDate, e.Location, e.DistanceKm, e.EventType, e.OrganiserID }).SingleOrDefaultAsync();
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateEvent(EventWriteRequest request)
    {
        var organiserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var item = new Event { EventName = request.EventName.Trim(), Description = request.Description.Trim(), EventDate = request.EventDate, Location = request.Location.Trim(), DistanceKm = request.DistanceKm, EventType = request.EventType, OrganiserID = organiserId };
        _context.Events.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvent), new { id = item.EventID }, new { item.EventID, item.EventName, item.Description, item.EventDate, item.Location, item.DistanceKm, item.EventType, item.OrganiserID });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateEvent(int id, EventWriteRequest request)
    {
        var item = await _context.Events.FindAsync(id);
        if (item == null) return NotFound();
        var organiserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (item.OrganiserID != organiserId) return Forbid();
        item.EventName = request.EventName.Trim(); item.Description = request.Description.Trim(); item.EventDate = request.EventDate;
        item.Location = request.Location.Trim(); item.DistanceKm = request.DistanceKm; item.EventType = request.EventType;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var item = await _context.Events.FindAsync(id);
        if (item == null) return NotFound();
        var organiserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (item.OrganiserID != organiserId) return Forbid();
        if (await _context.EventCategories.AnyAsync(ec => ec.EventID == id))
            return Conflict(new { message = "This event has categories or enrolments and cannot be deleted." });
        _context.Events.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
