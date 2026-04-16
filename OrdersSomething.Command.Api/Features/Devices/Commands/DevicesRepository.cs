using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public class DevicesRepository(MyDbContext dbContext) : IDevicesRepository
{
    public async Task<Models.Devices> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await dbContext.Devices.FindAsync(id);
    }

    public async Task SaveAsync(Models.Devices device, CancellationToken cancellationToken)
    {
        var entry = dbContext.Entry(device);
        if (entry.State == EntityState.Detached)
        {
            dbContext.Devices.Add(device);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}