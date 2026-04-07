using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public class DeleteDeviceHandler(MyDbContext dbContext) : IRequestHandler<DeleteDeviceCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await dbContext.Devices
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (device == null)
        {
            throw new KeyNotFoundException($"Property with id {request.Id} does not exist!");
        }

        device.IsDeleted = request.IsDeleted;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}