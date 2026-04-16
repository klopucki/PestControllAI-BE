using MassTransit;
using MediatR;
using OrdersSomething.Core.Events;
using OrdersSomething.Core.Exceptions;

namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public class UpsertDeviceHandler(IDevicesRepository repository, ITopicProducer<DeviceUpsertedEvent> producer)
    : IRequestHandler<UpsertDeviceCommand, UpsertDeviceResponse>
{
    public async Task<UpsertDeviceResponse> Handle(UpsertDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await GetOrAdd(request.Id, request.PropertiesId, cancellationToken) 
            ?? throw new EntityNotFoundException(nameof(Models.Devices), request.Id);

        device.upsert(request);

        await repository.SaveAsync(device, cancellationToken);

        await producer.Produce(new DeviceUpsertedEvent
        {
            Id = device.Id,
            Name = device.Name,
            Type = device.Type,
            Status = device.Status,
            IsListening = device.IsListening,
            IsDeleted = device.IsDeleted,
            LastHeartbeat = device.LastHeartbeat,
            CreatedAt = device.CreatedAt,
            PropertiesId = device.PropertiesId
        }, cancellationToken);

        return new UpsertDeviceResponse { Id = device.Id };
    }

    private async Task<Models.Devices> GetOrAdd(Guid id, Guid propertiesId, CancellationToken ct)
    {
        if (id != Guid.Empty)
        {
            var existing = await repository.GetByIdAsync(id, ct);
            if (existing != null) return existing;
        }

        return new Models.Devices
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            PropertiesId = propertiesId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
