
namespace OrdersSomething.Query.Api.Models;

public class Properties
{
    public Guid Id { get; set; } // pk
    public Guid UserId { get; set; } // fk
    public User User { get; set; } = null!; // nawigacja
    public string Name { get; set; } = string.Empty; // varchar 100
    public string Address { get; set; } = string.Empty; // text
    public string Description { get; set; } = string.Empty; // text
    public bool IsDeleted { get; set; } // boolean
    public DateTime CreatedAt { get; set; } // timestamp ???
    public ICollection<Devices> Devices { get; set; } = new List<Devices>();
}