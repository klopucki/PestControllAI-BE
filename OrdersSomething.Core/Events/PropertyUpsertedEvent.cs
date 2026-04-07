namespace OrdersSomething.Core.Events;

public record PropertyUpsertedEvent
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
}
