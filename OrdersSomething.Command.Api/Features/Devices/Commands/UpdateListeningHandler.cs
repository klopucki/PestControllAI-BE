using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;

namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public class UpdateListeningHandler(MyDbContext dbContext, ITopicProducer<DeviceListeningChangedEvent> producer) : IRequestHandler<UpdateListeningCommand, Unit>
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

        await producer.Produce(new DeviceListeningChangedEvent()
        {
            Id = device.Id,
            IsListening = device.IsListening,
        }, cancellationToken);
        
        return Unit.Value;
    }
}