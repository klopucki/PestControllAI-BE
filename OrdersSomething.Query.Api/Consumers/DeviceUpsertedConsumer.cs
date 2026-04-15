using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;
using OrdersSomething.Query.Api.Models; 

namespace OrdersSomething.Query.Api.Consumers;

public class DeviceUpsertedConsumer(MyDbContext dbContext) : IConsumer<DeviceUpsertedEvent>
{
    public async Task Consume(ConsumeContext<DeviceUpsertedEvent> context)
    {
        var message = context.Message;

        var device = await dbContext.Devices.FirstOrDefaultAsync(p => p.Id == message.Id);
        
        if (device == null)
        {
            device = new Devices()
            {
                Id = message.Id,
            };
            dbContext.Devices.Add(device);
        }

        device.Name = message.Name;
        device.Type = message.Type;
        device.Status = message.Status;
        device.IsListening = message.IsListening;
        device.IsDeleted = message.IsDeleted;
        device.LastHeartbeat = message.LastHeartbeat;
        device.CreatedAt = message.CreatedAt;

        device.PropertiesId = message.PropertiesId;

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
