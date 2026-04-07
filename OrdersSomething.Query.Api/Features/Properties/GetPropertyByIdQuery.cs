using MediatR;

namespace OrdersSomething.Query.Api.Features.Properties;

public class GetPropertyByIdQuery(Guid PropertyId) : IRequest<PropertiesDto?>
{
    public Guid PropertyId { get; set; } = PropertyId;
}