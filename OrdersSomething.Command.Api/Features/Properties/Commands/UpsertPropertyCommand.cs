using MediatR;

namespace OrdersSomething.Command.Api.Features.Properties.Commands;

public class UpsertPropertyCommand : IRequest<UpsertPropertyResponse>
{
    public Guid Id { get; set; }
    public String Name { get; set; } = String.Empty;
    public String Address { get; set; } = String.Empty;
    public String Description { get; set; } = String.Empty;
    public bool IsDeleted { get; set; }
}

public class UpsertPropertyResponse
{
    public Guid Id { get; set; }
} 