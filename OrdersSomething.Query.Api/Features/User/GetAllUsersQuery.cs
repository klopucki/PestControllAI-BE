using MediatR;

namespace OrdersSomething.Query.Api.Features.User;

public class GetAllUsersQuery : IRequest<List<UserDto>>
{
    
}

public class UserDto
{    
    public Guid Id { get; set; }
    public String Firstname { get; set; }
    public String Surname { get; set; }
    
}