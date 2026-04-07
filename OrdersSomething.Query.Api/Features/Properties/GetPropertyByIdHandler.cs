using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Query.Api.Features.Properties;

public class GetPropertyByIdHandler(MyDbContext dbContext) : IRequestHandler<GetPropertyByIdQuery, PropertiesDto?>
{
    public async Task<PropertiesDto?> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
    {
        var property = await dbContext.Properties
            .Where(p => p.Id == request.PropertyId)
            .ProjectToType<PropertiesDto>()
            .FirstOrDefaultAsync(cancellationToken);
        
        return property;
    }
}