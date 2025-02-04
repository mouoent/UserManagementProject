using MediatR;
using UserManagementProject.Application.Features.User.DTOs;

namespace UserManagementProject.Application.Features.User.Queries;

public record GetAllUsersQuery: IRequest<List<UserDto>> {}
