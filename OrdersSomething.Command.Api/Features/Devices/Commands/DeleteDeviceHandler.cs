using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;
using OrdersSomething.Tests.Exceptions;

namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public class DeleteDeviceHandler(MyDbContext dbContext, ITopicProducer<DeviceDeletedEvent> producer) : IRequestHandler<DeleteDeviceCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await dbContext.Devices
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (device == null)
        {
            throw new EntityNotFoundException(nameof(DeviceDeletedEvent), request.Id);
        }

        device.IsDeleted = request.IsDeleted;

        await dbContext.SaveChangesAsync(cancellationToken);

        await producer.Produce(new DeviceDeletedEvent()
        {
            Id = device.Id,
            IsDeleted = device.IsDeleted,
        }, cancellationToken);
        
        return Unit.Value;
    }
}