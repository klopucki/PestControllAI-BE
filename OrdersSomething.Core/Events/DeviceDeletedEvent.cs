namespace OrdersSomething.Core.Events;

public class DeviceDeletedEvent
{
    public Guid Id { get; init; }
    public bool IsDeleted { get; init; }
}