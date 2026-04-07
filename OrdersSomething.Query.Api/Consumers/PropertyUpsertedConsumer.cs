using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;
using OrdersSomething.Query.Api.Models; // Poprawiona przestrzeń nazw

namespace OrdersSomething.Query.Api.Consumers;

public class PropertyUpsertedConsumer(MyDbContext dbContext) : IConsumer<PropertyUpsertedEvent>
{
    public async Task Consume(ConsumeContext<PropertyUpsertedEvent> context)
    {
        var message = context.Message;

        var property = await dbContext.Properties.FirstOrDefaultAsync(p => p.Id == message.Id);
        
        if (property == null)
        {
            property = new Properties
            {
                Id = message.Id,
                // W bazie Query również musimy mieć UserId, jeśli to ten sam schemat
                UserId = new Guid("99999999-9999-9999-9999-999999999991") 
            };
            dbContext.Properties.Add(property);
        }

        property.Name = message.Name;
        property.Address = message.Address;
        property.Description = message.Description;
        property.IsDeleted = message.IsDeleted;

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
