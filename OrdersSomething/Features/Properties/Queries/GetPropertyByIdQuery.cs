using MediatR;

namespace OrdersSomething.Features.Properties.Queries;

public class GetPropertyByIdQuery(Guid PropertyId) : IRequest<PropertiesDto?>
{
    public Guid PropertyId { get; set; } = PropertyId;
}