using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public class UpsertDeviceHandler(MyDbContext dbContext) : IRequestHandler<UpsertDeviceCommand>
{
    public async Task Handle(UpsertDeviceCommand request, CancellationToken cancellationToken)
    {
        var property = await GetOrAdd(request.Id, request.PropertyId, cancellationToken);

        property.upsert(request);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Models.Devices> GetOrAdd(Guid id, Guid PropertiesId, CancellationToken ct)
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
            PropertiesId = PropertiesId, 
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Devices.Add(@new);
        
        return @new;
    }
}