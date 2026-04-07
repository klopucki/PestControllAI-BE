using MediatR;

namespace OrdersSomething.Query.Api.Features.DeviceEvents;

public class GetDeviceEventsByDeviceIdQuery(Guid DeviceId): IRequest<List<DeviceEventsDto>>
{
    public Guid DeviceId { get; set; } = DeviceId;
}

public class DeviceEventsDto
{
    public Guid Id { get; set; } // pk
    public Guid DeviceId { get; set; } // fk
    public String? EventType { get; set; } // enum ('motion', 'sound', 'capture', 'alert')
    public String? Description { get; set; } // text
    public String? ImageUrl { get; set; } // varchar 512 (Link do S3/Cloudinary ze zdjęciem szkodnika)
    public int Severity { get; set; } // integer (1-5, gdzie 5 to krytyczny alert)
    public DateTime CreatedAt { get; set; } // timestamp (Kiedy wystąpiło zdarzenie)
}