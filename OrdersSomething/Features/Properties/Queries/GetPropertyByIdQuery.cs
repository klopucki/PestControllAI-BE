using MediatR;

namespace OrdersSomething.Features.Properties.Queries;

public class GetPropertyByIdQuery(Guid PropertyId) : IRequest<List<PropertiesDto>>
{
    public Guid PropertyId { get; set; } = PropertyId;
}