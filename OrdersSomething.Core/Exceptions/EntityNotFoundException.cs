namespace OrdersSomething.Tests.Exceptions;

public class EntityNotFoundException(string name, Guid id) : Exception($"Entity {name} with id {id} not found!")
{
    
}