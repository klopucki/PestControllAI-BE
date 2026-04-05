using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Models;

namespace OrdersSomething.Features.Properties.Commands;

public class UpsertPropertyHandler(MyDbContext dbContext) : IRequestHandler<UpsertPropertyCommand>
{
    public async Task Handle(UpsertPropertyCommand request, CancellationToken cancellationToken)
    {
        var property = await GetOrAdd(request.Id, cancellationToken);

        property.upsert(request);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Models.Properties> GetOrAdd(Guid id, CancellationToken ct)
    {
        if (id != Guid.Empty)
        {
            var existing = await dbContext.Properties
                .FirstOrDefaultAsync(p => p.Id == id, ct);
            
            if (existing != null) return existing;
        }

        var @new = new Models.Properties 
        { 
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            UserId = new Guid("99999999-9999-9999-9999-999999999991"), // TODO: fetch from JWT
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Properties.Add(@new);
        
        return @new;
    }
}