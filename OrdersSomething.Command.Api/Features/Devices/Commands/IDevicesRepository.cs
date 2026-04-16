namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public interface IDevicesRepository
{
    Task<Models.Devices> GetByIdAsync(Guid id, CancellationToken ct);
    Task SaveAsync(Models.Devices device, CancellationToken cancellationToken);
}