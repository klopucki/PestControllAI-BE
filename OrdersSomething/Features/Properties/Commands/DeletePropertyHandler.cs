using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Features.Properties.Commands;

public class DeletePropertyHandler(MyDbContext dbContext) : IRequestHandler<DeletePropertyCommand, Unit>
{
    public async Task<Unit> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
    {
        Models.Properties? property = await dbContext.Properties
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (property == null)
        {
            throw new KeyNotFoundException($"Property with id {request.Id} does not exist!");
        }

        property.IsDeleted = request.IsDeleted;
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}