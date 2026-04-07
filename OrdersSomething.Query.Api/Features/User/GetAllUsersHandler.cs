using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrdersSomething.Query.Api.Features.User;

public class GetAllUsersHandler: IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    
    private readonly MyDbContext _dbContext;

    public GetAllUsersHandler(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var items = await _dbContext.Users
            .Select(i => new UserDto()
            {
                Id = i.Id,
                Firstname = i.Firstname,
                Surname = i.Surname
            }).ToListAsync(cancellationToken);
        
        return items;
    }
}