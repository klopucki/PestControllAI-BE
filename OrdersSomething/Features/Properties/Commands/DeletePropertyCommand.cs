using MediatR;

namespace OrdersSomething.Features.Properties.Commands;

public class DeletePropertyCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }

}