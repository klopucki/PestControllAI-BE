using MassTransit;
using MassTransit.KafkaIntegration.Activities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;

namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public class UpsertDeviceHandler(MyDbContext dbContext, ITopicProducer<DeviceUpsertedEvent> producer)
    : IRequestHandler<UpsertDeviceCommand, UpsertDeviceResponse>
{
    public async Task<UpsertDeviceResponse> Handle(UpsertDeviceCommand request, CancellationToken cancellationToken)
    {
        var property = await GetOrAdd(request.Id, request.PropertiesId, cancellationToken);

        property.upsert(request);

        await dbContext.SaveChangesAsync(cancellationToken);

        await producer.Produce(new DeviceUpsertedEvent
        {
            Id = property.Id,
            Name = property.Name,
            Type = property.Type,
            Status = property.Status,
            IsListening = property.IsListening,
            IsDeleted = property.IsDeleted,
            LastHeartbeat = property.LastHeartbeat,
            CreatedAt = property.CreatedAt,
            
            PropertiesId = property.PropertiesId
        }, cancellationToken);

        return new UpsertDeviceResponse()
        {
            Id = property.Id
        };
    }

    private async Task<Models.Devices> GetOrAdd(Guid id, Guid propertiesId, CancellationToken ct)
    {
        if (id != Guid.Empty)
        {
            var existing = await dbContext.Devices
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (existing != null) return existing;
        }

        var @new = new Models.Devices()
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            PropertiesId = propertiesId,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Devices.Add(@new);

        return @new;
    }
}