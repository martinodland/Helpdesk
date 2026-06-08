namespace HelpDesk.Dtos;

public class ResponseAdminSettingDto
{
    public required string StatusOverview { get; set; }

    public required string ShowMyTickets { get; set; }

    public required string ShowNewestTickets { get; set; }
}