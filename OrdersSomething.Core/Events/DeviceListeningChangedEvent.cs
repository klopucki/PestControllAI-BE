namespace OrdersSomething.Core.Events;

public class DeviceListeningChangedEvent
{
    public Guid Id { get; init; }
    public bool IsListening { get; init; }
}