using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;
using OrdersSomething.Core.Exceptions;
using OrdersSomething.Query.Api.Models;

namespace OrdersSomething.Query.Api.Consumers;

public class DeviceListeningChangedConsumer(MyDbContext dbContext) : IConsumer<DeviceListeningChangedEvent>
{
    public async Task Consume(ConsumeContext<DeviceListeningChangedEvent> context)
    {
        var message = context.Message;

        var devices = await dbContext.Devices.FirstOrDefaultAsync(p => p.Id == message.Id)
                       ?? throw new EntityNotFoundException(nameof(Devices), message.Id);

        devices.IsListening = message.IsListening;

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}