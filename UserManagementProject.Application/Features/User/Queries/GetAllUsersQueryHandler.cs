using MediatR;
using UserManagementProject.Application.Features.User.DTOs;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.User.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result = _userRepository.GetAllUsersAsync();        

        return result;
    }
}
