using MassTransit;
using MediatR;
using OrdersSomething.Core.Events;

namespace OrdersSomething.Command.Api.Features.Properties.Commands;

public class UpsertPropertyHandler(IPropertiesRepository repository, ITopicProducer<PropertyUpsertedEvent> producer)
    : IRequestHandler<UpsertPropertyCommand, UpsertPropertyResponse>
{
    public async Task<UpsertPropertyResponse> Handle(UpsertPropertyCommand request, CancellationToken cancellationToken)
    {
        var property = await GetOrAdd(request.Id, cancellationToken);

        property.upsert(request);

        await repository.SaveAsync(property, cancellationToken);

        await producer.Produce(new PropertyUpsertedEvent
        {
            Id = property.Id,
            Name = property.Name,
            Address = property.Address,
            Description = property.Description,
            IsDeleted = property.IsDeleted
        }, cancellationToken);

        return new UpsertPropertyResponse { Id = property.Id };
    }

    private async Task<Models.Properties> GetOrAdd(Guid id, CancellationToken ct)
    {
        if (id != Guid.Empty)
        {
            return await repository.GetByIdAsync(id, ct);
        }

        return new Models.Properties
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            UserId = new Guid("99999999-9999-9999-9999-999999999991"),
            CreatedAt = DateTime.UtcNow
        };
    }
}