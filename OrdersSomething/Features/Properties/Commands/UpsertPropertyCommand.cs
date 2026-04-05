using MediatR;

namespace OrdersSomething.Features.Properties.Commands;

public class UpsertPropertyCommand : IRequest
{
    public Guid Id { get; set; }
    public String Name { get; set; } = String.Empty;
    public String Address { get; set; } = String.Empty;
    public String Description { get; set; } = String.Empty;
    public bool IsDeleted { get; set; }
}