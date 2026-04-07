using OrdersSomething.Command.Api.Features.Devices.Commands;

namespace OrdersSomething.Command.Api.Models;

public class Devices
{
    public Guid Id { get; set; } // pk
    public Guid PropertiesId { get; set; } // fk
    public Properties Properties { get; set; } = null!; // navigation
    public string Name { get; set; } = string.Empty; // varchar 100
    public string Type { get; set; } = string.Empty; // enum ('camera', 'microphone', 'sensor', 'trap')
    public string Status { get; set; } = string.Empty; // enum ('active', 'inactive', 'alert')
    public bool IsListening { get; set; } // boolean (default: true)
    public bool IsDeleted { get; set; } // boolean (default: false)
    public DateTime LastHeartbeat { get; set; } // timestamp
    public DateTime CreatedAt { get; set; } // timestamp

    public ICollection<DeviceEvents> DeviceEvents { get; set; } = new List<DeviceEvents>();

    public void upsert(UpsertDeviceCommand request)
    {
        Name = request.Name;
        Type = request.Type;
        Status = request.Status;
        IsListening = request.IsListening;
        IsDeleted = request.IsDeleted;
        LastHeartbeat = request.LastHeartbeat;
        CreatedAt = request.CreatedAt;
    }
}