namespace OrdersSomething.Core.Events;

public class DeviceUpsertedEvent
{
    public Guid Id { get; set; } // pk
    public Guid PropertiesId { get; set; } // fk
    public string Name { get; set; } = string.Empty; // varchar 100
    public string Type { get; set; } = string.Empty; // enum ('camera', 'microphone', 'sensor', 'trap')
    public string Status { get; set; } = string.Empty; // enum ('active', 'inactive', 'alert')
    public bool IsListening { get; set; } // boolean (default: true)
    public bool IsDeleted { get; set; } // boolean (default: false)
    public DateTime LastHeartbeat { get; set; } // timestamp
    public DateTime CreatedAt { get; set; } // timestamp
}