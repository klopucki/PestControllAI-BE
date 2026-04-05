using MediatR;

namespace OrdersSomething.Features.Devices.Commands;

public class DeleteDeviceCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }

}