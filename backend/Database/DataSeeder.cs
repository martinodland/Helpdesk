using HelpDesk.Models;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Database;

public static class DataSeeder
{
    public static void Seed(ApplicationDbContext _dbContext)
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        var users = new List<User>
        {
            new() { Name = "Martin Odland", Email = "test@gmail.com", PasswordHash = new PasswordHasher<User>().HashPassword(null!, "test"), Role = "Admin" },
            new() { Name = "Test bruker", Email = "test2@gmail.com", PasswordHash = new PasswordHasher<User>().HashPassword(null!, "test"), Role = "User"}
        };

        _dbContext.Users.AddRange(users);
        _dbContext.SaveChanges();

        var tickets = new List<Ticket>
        {
            new() { Title = "Login page broken", Description = "Users can't log in after the last deploy.", Status = "Open", Priority = "High", CreatedByUserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new() { Title = "Password reset email not sending", Description = "Reset emails go to spam or don't arrive at all.", Status = "InProgress", Priority = "Normal", CreatedByUserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new() { Title = "Dashboard loads slowly", Description = "Takes 8+ seconds on first load.", Status = "Closed", Priority = "Low", CreatedByUserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new() { Title = "Export to CSV fails", Description = "500 error when exporting more than 1000 rows.", Status = "Closed", Priority = "High", CreatedByUserId = users[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },
            new() { Title = "Update favicon", Description = "Use the new brand logo.", Status = "Open", Priority = "Low", CreatedByUserId = users[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-10), ClosedAt = DateTime.UtcNow.AddDays(-7) },
        };

        _dbContext.Tickets.AddRange(tickets);
        _dbContext.SaveChanges();

        var settings = new List<Setting>
        {
            new() { ShowMyTickets = "true", ShowNewestTickets = "true", StatusOverview = "true", UserId = users[0].Id},
            new() { ShowMyTickets = "true", ShowNewestTickets = "false", StatusOverview = "true", UserId = users[1].Id}
        };

        _dbContext.Settings.AddRange(settings);
        _dbContext.SaveChanges();
    }
}