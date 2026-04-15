using MediatR;

namespace OrdersSomething.Query.Api.Features.Devices;

public class GetDevicesByPropertyIdQuery(Guid propertiesId) : IRequest<List<DeviceDto>>
{
    public Guid PropertiesId { get; set; } = propertiesId; // primary constructor
}