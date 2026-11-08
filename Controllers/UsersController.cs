using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday_POE2.Data;
using Raceday_POE2.Models;

namespace Raceday_POE2.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Organiser,Participant")]
public class UsersController : ControllerBase
{
    private readonly RaceDayContext _context;
    public UsersController(RaceDayContext context) => _context = context;

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _context.Users.AsNoTracking().Where(u => u.UserID == id)
            .Select(u => new { u.UserID, u.FirstName, u.LastName, u.Email, u.Role }).SingleOrDefaultAsync();
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(UpdateProfileRequest request)
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        var email = request.Email.Trim().ToLowerInvariant();
        if (await _context.Users.AnyAsync(u => u.UserID != id && u.Email.ToLower() == email))
            return Conflict(new { message = "That email address is already in use." });
        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.Email = email;
        await _context.SaveChangesAsync();
        return Ok(new { user.UserID, user.FirstName, user.LastName, user.Email, user.Role });
    }
}
