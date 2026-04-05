using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Features.Properties.Queries;

public class GetPropertyByIdHandler(MyDbContext dbContext): IRequestHandler<GetPropertyByIdQuery, List<PropertiesDto>>
{
    public async Task<List<PropertiesDto>> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
    {
        // fixme move to repo
        var properties = await dbContext.Devices.Where(d => d.PropertiesId == request.PropertyId)
            .ProjectToType<PropertiesDto>()
            .ToListAsync(cancellationToken);
        
        return properties;
    }
}