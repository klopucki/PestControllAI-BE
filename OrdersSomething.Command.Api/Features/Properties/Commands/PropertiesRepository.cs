using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Command.Api.Features.Properties.Commands;

public class PropertiesRepository(MyDbContext dbContext) : IPropertiesRepository
{
    public async Task<Models.Properties> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await dbContext.Properties.FindAsync(id);
    }

    public async Task SaveAsync(Models.Properties property, CancellationToken ct)
    {
        var entry = dbContext.Entry(property);
        if (entry.State == EntityState.Detached)
        {
            dbContext.Properties.Add(property);
        }

        await dbContext.SaveChangesAsync(ct);
    }
}