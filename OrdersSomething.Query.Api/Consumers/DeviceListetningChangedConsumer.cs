using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;
using OrdersSomething.Query.Api.Models;
using OrdersSomething.Tests.Exceptions;

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