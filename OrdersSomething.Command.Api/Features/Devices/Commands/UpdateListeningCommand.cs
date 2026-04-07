using MediatR;

namespace OrdersSomething.Command.Api.Features.Devices.Commands;

public class UpdateListeningCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public bool IsListening { get; set; }
}