using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersSomething.Tests.Exceptions;

namespace OrdersSomething.Query.Api.Features.Devices;

public class GetDeviceByIdHandler(MyDbContext dbContext) : IRequestHandler<GetDevicesByIdQuery, DeviceDto>
{
    public async Task<DeviceDto> Handle(GetDevicesByIdQuery request, CancellationToken cancellationToken)
    {
        // fixme move to repo
        var device = await dbContext.Devices.Where(d => d.Id == request.Id)
                         .ProjectToType<DeviceDto>().FirstOrDefaultAsync(cancellationToken)
                     ?? throw new EntityNotFoundException(nameof(Devices), request.Id);

        return device;
    }
}