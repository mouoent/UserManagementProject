using MediatR;
using UserManagementProject.Application.Features.User.DTOs;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.User.Queries;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsDto>
{
    private IUserRepository _userRepository;

    public GetUserDetailsQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDetailsDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetUserDetailsByIdAsync(request.Id);

        if (result is null)
            throw new KeyNotFoundException("User not found");

        return result;
    }
}
