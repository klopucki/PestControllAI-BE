using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Features.Devices.Commands;

public class UpdateListeningHandler(MyDbContext dbContext) : IRequestHandler<UpdateListeningCommand, Unit>
{
    public async Task<Unit> Handle(UpdateListeningCommand request, CancellationToken cancellationToken)
    {
        var device = await dbContext.Devices
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (device == null)
        {
            throw new KeyNotFoundException($"Property with id {request.Id} does not exist!");
        }

        device.IsListening = request.IsListening;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}