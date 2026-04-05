using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Features.Devices.Query;

namespace OrdersSomething.Features.DeviceEvents.Queries;

public class GetDeviceEventsByDeviceIdHandler(MyDbContext dbContext)
    : IRequestHandler<GetDeviceEventsByDeviceIdQuery, List<DeviceEventsDto>>
{
    public async Task<List<DeviceEventsDto>> Handle(GetDeviceEventsByDeviceIdQuery request,
        CancellationToken cancellationToken)
    {
        {
            // fixme move to repo
            var devices = await dbContext.DeviceEvents.Where(d => d.DeviceId == request.DeviceId)
                .ProjectToType<DeviceEventsDto>()
                .ToListAsync(cancellationToken);

            return devices;
        }
    }
}