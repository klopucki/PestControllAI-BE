using MassTransit;
using MediatR;
using OrdersSomething.Core.Events;
using OrdersSomething.Core.Exceptions;

namespace OrdersSomething.Command.Api.Features.Properties.Commands;

public class DeletePropertyHandler(IPropertiesRepository repository, ITopicProducer<PropertyDeletedEvent> producer)
    : IRequestHandler<DeletePropertyCommand, Unit>
{
    public async Task<Unit> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = await repository.GetByIdAsync(request.Id, cancellationToken)
                       ?? throw new EntityNotFoundException(nameof(Properties), request.Id);

        property.IsDeleted = request.IsDeleted;

        await repository.SaveAsync(property, cancellationToken);

        await producer.Produce(new PropertyDeletedEvent
        {
            Id = property.Id,
            IsDeleted = property.IsDeleted
        }, cancellationToken);

        return Unit.Value;
    }
}