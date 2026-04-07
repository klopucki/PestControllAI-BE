using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Query.Api.Features.Properties;

public class GetAllPropertiesHandler: IRequestHandler<GetAllPropertiesQuery, List<PropertiesDto>>
{
    private readonly MyDbContext _dbContext;
    
    public GetAllPropertiesHandler(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<PropertiesDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
    {
        var items = await _dbContext.Properties
            .ProjectToType<PropertiesDto>()
            .ToListAsync(cancellationToken);

        return items;
    }
}