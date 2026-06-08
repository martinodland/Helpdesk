using System.Security.Claims;
using HelpDesk.Dtos;
using HelpDesk.Dtos.Settings;
using HelpDesk.Filters;
using HelpDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Controllers;

[ApiController]
[Route("settings")]
public class SettingController(ApplicationDbContext _dbContext): ControllerBase
{
    // Get the users settings.

    [HttpGet]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> GetUserSettings()
    {
        int userId = int.Parse(User.FindFirstValue("id")!);

        string userRole = User.FindFirstValue("role")!;

        Setting? settings = await _dbContext.Settings.FirstOrDefaultAsync(searchedSettings => searchedSettings.UserId == userId);

        if(userRole == "User")
        {
            return Ok( new { message = "Settings retrieved successfully!" , settings = MapToDto2(settings!)} );
        }

        return Ok( new { message = "Settings retrieved successfully!" , settings = MapToDto(settings!)} );
    }

    // Update the users settings.

    [HttpPatch]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> UpdateUserSettings(UpdateSettingsDto dto)
    {
        int userId = int.Parse(User.FindFirstValue("id")!);

        Setting? settings = await _dbContext.Settings.FirstAsync(searchedSettings => searchedSettings.UserId == userId);

        if (dto.ShowMyTickets is not null) settings.ShowMyTickets = dto.ShowMyTickets;

        if (dto.ShowNewestTickets is not null) settings.ShowNewestTickets = dto.ShowNewestTickets;

        if (dto.StatusOverview is not null) settings.StatusOverview = dto.StatusOverview;

        await _dbContext.SaveChangesAsync();

        return Ok( new { message = "Settings updated successfully!", settings = settings} );
    }

    // The admin settings response.

    private static ResponseAdminSettingDto MapToDto(Setting settings) => new()
    {
        StatusOverview = settings.StatusOverview,
        ShowNewestTickets = settings.ShowNewestTickets,
        ShowMyTickets = settings.ShowMyTickets
    };

    // The user settings response.

    private static ResponseUserSettingDto MapToDto2(Setting settings) => new()
    {
        StatusOverview = settings.StatusOverview,
        ShowMyTickets = settings.ShowMyTickets
    };
}