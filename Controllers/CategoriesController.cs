using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday_POE2.Data;
using Raceday_POE2.Models;

namespace Raceday_POE2.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize(Roles = "Organiser,Participant")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayContext _context;
    public CategoriesController(RaceDayContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetCategories() => Ok(await _context.Categories.AsNoTracking()
        .Select(c => new { c.CategoryID, c.CategoryName, c.DistanceKm, c.CategoryType }).ToListAsync());

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateCategory(CategoryWriteRequest request)
    {
        var item = new Category { CategoryName = request.CategoryName.Trim(), DistanceKm = request.DistanceKm, CategoryType = request.CategoryType };
        _context.Categories.Add(item);
        await _context.SaveChangesAsync();
        return Created($"/api/categories/{item.CategoryID}", new { item.CategoryID, item.CategoryName, item.DistanceKm, item.CategoryType });
    }
}
