using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Query.Api.Features.Devices;

public class GetDevicecbyPropertyIdHandler(MyDbContext dbContext) : IRequestHandler<GetDevicesByPropertyIdQuery, List<DeviceDto>>
{
    public async Task<List<DeviceDto>> Handle(GetDevicesByPropertyIdQuery request, CancellationToken cancellationToken)
    {
        // fixme move to repo
        var devices = await dbContext.Devices.Where(d => d.PropertiesId == request.PropertiesId)
            .ProjectToType<DeviceDto>()
            .ToListAsync(cancellationToken);
        
        return devices;
    }
}