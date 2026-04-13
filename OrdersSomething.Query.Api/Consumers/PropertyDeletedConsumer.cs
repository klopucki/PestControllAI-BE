using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;
using OrdersSomething.Query.Api.Models;
using OrdersSomething.Tests.Exceptions;

namespace OrdersSomething.Query.Api.Consumers;

public class PropertyDeletedConsumer(MyDbContext dbContext) : IConsumer<PropertyDeletedEvent>
{
    public async Task Consume(ConsumeContext<PropertyDeletedEvent> context)
    {
        var message = context.Message;

        var property = await dbContext.Properties.FirstOrDefaultAsync(p => p.Id == message.Id)
                       ?? throw new EntityNotFoundException(nameof(Properties), message.Id);

        property.IsDeleted = message.IsDeleted;

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}