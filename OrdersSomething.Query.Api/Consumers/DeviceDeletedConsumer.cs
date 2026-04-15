using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;
using OrdersSomething.Query.Api.Models;
using OrdersSomething.Tests.Exceptions;

namespace OrdersSomething.Query.Api.Consumers;

public class DeviceDeletedConsumer(MyDbContext dbContext) : IConsumer<DeviceDeletedEvent>
{
    public async Task Consume(ConsumeContext<DeviceDeletedEvent> context)
    {
        var message = context.Message;

        var devices = await dbContext.Devices.FirstOrDefaultAsync(p => p.Id == message.Id)
                       ?? throw new EntityNotFoundException(nameof(Devices), message.Id);

        devices.IsDeleted = message.IsDeleted;

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}