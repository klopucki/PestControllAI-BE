namespace OrdersSomething.Models;

public class User
{
    public Guid Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public ICollection<Properties> Properties { get; set; } = new List<Properties>();
}