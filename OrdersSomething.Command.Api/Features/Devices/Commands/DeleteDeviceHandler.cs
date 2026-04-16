using MassTransit;
using MediatR;
using OrdersSomething.Core.Events;
using OrdersSomething.Core.Exceptions;

namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public class DeleteDeviceHandler(IDevicesRepository repository, ITopicProducer<DeviceDeletedEvent> producer) : IRequestHandler<DeleteDeviceCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Devices), request.Id);

        device.IsDeleted = request.IsDeleted;

        await repository.SaveAsync(device, cancellationToken);

        await producer.Produce(new DeviceDeletedEvent
        {
            Id = device.Id,
            IsDeleted = device.IsDeleted,
        }, cancellationToken);
        
        return Unit.Value;
    }
}
