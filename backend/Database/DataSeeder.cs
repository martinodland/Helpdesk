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
            new() { Name = "Test bruker", Email = "test2@gmail.com", PasswordHash = new PasswordHasher<User>().HashPassword(null!, "test"), Role = "Admin"}
        };

        _dbContext.Users.AddRange(users);
        _dbContext.SaveChanges();

        var tickets = new List<Ticket>
        {
            new() { Title = "Login page broken", Description = "Users can't log in after the last deploy.", Status = "Åpen", Priority = "Høy", CreadtedByUserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new() { Title = "Password reset email not sending", Description = "Reset emails go to spam or don't arrive at all.", Status = "Påbegynt", Priority = "Normal", CreadtedByUserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new() { Title = "Dashboard loads slowly", Description = "Takes 8+ seconds on first load.", Status = "Fullført", Priority = "Lav", CreadtedByUserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new() { Title = "Export to CSV fails", Description = "500 error when exporting more than 1000 rows.", Status = "Fullført", Priority = "Høy", CreadtedByUserId = users[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },
            new() { Title = "Update favicon", Description = "Use the new brand logo.", Status = "Åpen", Priority = "Lav", CreadtedByUserId = users[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-10), ClosedAt = DateTime.UtcNow.AddDays(-7) },
        };

        _dbContext.Tickets.AddRange(tickets);
        _dbContext.SaveChanges();
    }
}