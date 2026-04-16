namespace OrdersSomething.Command.Api.Features.Properties.Commands;

public interface IPropertiesRepository
{
    public Task<Models.Properties> GetByIdAsync(Guid id, CancellationToken ct);

    Task SaveAsync(Models.Properties property, CancellationToken cancellationToken);
}