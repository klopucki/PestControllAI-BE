
namespace OrdersSomething.Command.Api.Models;

// todo change all model classes to singlular names
public class DeviceEvents
{
    public Guid Id { get; set; } // pk
    public Guid DeviceId { get; set; } // fk
    public Devices Device { get; set; } // navigation property
    public String? EventType { get; set; } // enum ('motion', 'sound', 'capture', 'alert')
    public String? Description { get; set; } // text
    public String? ImageUrl { get; set; } // varchar 512 (Link do S3/Cloudinary ze zdjęciem szkodnika)
    public int Severity { get; set; } // integer (1-5, gdzie 5 to krytyczny alert)
    public DateTime CreatedAt { get; set; } // timestamp (Kiedy wystąpiło zdarzenie)
}