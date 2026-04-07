namespace OrdersSomething.Query.Api.Models;

public class UserNotifications
{
    public Guid Id { get; set; } // pk
    public Guid UserId { get; set; } // fk
    public User User { get; set; } = null!; // navigation
    public Guid EventId { get; set; } // fk
    public DeviceEvents Event { get; set; } = null!; // navigation
    public string Title { get; set; } = string.Empty; // varchar 255
    public string Body { get; set; } = string.Empty; // text
    public bool IsRead { get; set; } = false; // boolean (default: false)
    public DateTime SentAt { get; set; } = DateTime.UtcNow; // timestamp
}