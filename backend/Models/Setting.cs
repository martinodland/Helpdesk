namespace HelpDesk.Models;

public class Setting
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string StatusOverview { get; set; } = "true";

    public string ShowNewestTickets { get; set; } = "false";

    public string ShowMyTickets { get; set; } = "true";
}