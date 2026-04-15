using MediatR;

namespace OrdersSomething.Query.Api.Features.Devices;

public class GetDevicesByIdQuery(Guid id): IRequest<DeviceDto>
{
    public Guid Id { get; set; } = id;
}