using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Core.Events;
using OrdersSomething.Tests.Exceptions;

namespace OrdersSomething.Command.Api.Features.Properties.Commands;

public class DeletePropertyHandler(MyDbContext dbContext, ITopicProducer<PropertyDeletedEvent> producer)
    : IRequestHandler<DeletePropertyCommand, Unit>
{
    public async Task<Unit> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
    {
        Models.Properties property =
            await dbContext.Properties.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Properties), request.Id);

        property.IsDeleted = request.IsDeleted;

        await dbContext.SaveChangesAsync(cancellationToken);

        await producer.Produce(new PropertyDeletedEvent
        {
            Id = property.Id,
            IsDeleted = property.IsDeleted
        }, cancellationToken);

        return Unit.Value;
    }
}